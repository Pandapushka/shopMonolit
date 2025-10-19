using Microsoft.EntityFrameworkCore;
using shop.Common;
using shop.Data;
using shop.Model.Entitys.Order;
using shop.ModelDTO.OrderDTO;
using shop.Seed;
using shop.Services.CartService;
using shop.Services.EmailService;
using System.Linq;

namespace shop.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICartService _cartService;
        private readonly IEmailService _emailService;

        public OrderService(AppDbContext appDbContext, ICartService cartService, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _cartService = cartService;
            _emailService = emailService;
        }

        public async Task<Order> CreateOrderFromCartAsync(OrderCreateFromCartDTO orderDto)
        {
            if (orderDto == null)
                throw new ArgumentNullException(nameof(orderDto));

            if (string.IsNullOrWhiteSpace(orderDto.UserID))
                throw new ArgumentException("UserID обязателен для создания заказа", nameof(orderDto.UserID));

            var userId = orderDto.UserID;

            var transaction = await _appDbContext.Database.BeginTransactionAsync();

            try
            {

                var user = await _appDbContext.AppUsers.FirstOrDefaultAsync(i => i.Id == userId);
                
                if (user == null)
                    throw new InvalidOperationException("Не корректный пользователь");

                if(!user.EmailConfirmed)
                    throw new InvalidOperationException("Пользователь с неподтвержденным Email не может делать заказы");



                var cart = await _appDbContext.Carts
                    .Include(c => c.Items)
                        .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.UserID == userId);

                if (cart == null || !cart.Items.Any())
                    throw new InvalidOperationException("Корзина пуста или не существует");

                foreach (var item in cart.Items)
                {
                    if (item.Product == null)
                        throw new InvalidOperationException($"Товар с ID {item.ProductId} не найден в базе данных.");
                }

                double orderTotalAmount = cart.Items.Sum(i => i.Quantity * i.Product.Price);
                int totalCount = cart.Items.Sum(i => i.Quantity);

                var order = new Order
                {
                    CustomerName = orderDto.CustomerName ?? throw new ArgumentException("Имя клиента обязательно"),
                    CustomerEmail = user.Email,
                    CustomerAddress = orderDto.CustomerAddress,
                    AppUserId = userId,
                    OrderTotalAmount = orderTotalAmount,
                    TotalCount = totalCount,
                    Status = OrderStatus.Accepted,
                    OrderDateTime = DateTime.UtcNow,
                    OrderDetailItems = new List<OrderDetails>()
                };

                await _appDbContext.Order.AddAsync(order);
                await _appDbContext.SaveChangesAsync(); 

                foreach (var cartItem in cart.Items)
                {
                    var orderDetail = new OrderDetails
                    {
                        OrderId = order.Id,
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        ItemName = cartItem.Product.Name,
                        Price = cartItem.Product.Price
                    };

                    await _appDbContext.OrderDetails.AddAsync(orderDetail);
                }

                await _appDbContext.SaveChangesAsync();
                await _cartService.ClearCartAsync(userId);

                await _emailService.SendOrderConfirmationEmailAsync(
                    userEmail: order.CustomerEmail,
                    userName: order.CustomerName,
                    orderId: order.Id,
                    totalAmount: order.OrderTotalAmount,
                    orderDate: order.OrderDateTime
                );
                await transaction.CommitAsync();
                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Order> GetById(int id)
        {
            try 
            {
                return await _appDbContext.Order.Include(d => d.OrderDetailItems).ThenInclude(p => p.Product)
                    .FirstOrDefaultAsync(u => u.Id == id);   
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении заказа: {ex.Message}", ex);
            }
        }

        public async Task<List<Order>> GetByUserId(string id)
        {
            try
            {
                return await _appDbContext.Order.Include(d => d.OrderDetailItems).ThenInclude(p => p.Product)
                    .Where(u => u.AppUserId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении заказа: {ex.Message}", ex);
            }
        }

        public async Task<Order> UpdateOrderAsync(int id, OrderUpdateDTO orderUpdateDTO)
        {
            try
            {
                var existingOrder = await _appDbContext.Order
                    .Include(d => d.OrderDetailItems)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (existingOrder == null)
                {
                    throw new Exception($"Заказ с ID {id} не найден");
                }

                if (!string.IsNullOrEmpty(orderUpdateDTO.CustomerName))
                    existingOrder.CustomerName = orderUpdateDTO.CustomerName;

                if (!string.IsNullOrEmpty(orderUpdateDTO.CustomerEmail))
                    existingOrder.CustomerEmail = orderUpdateDTO.CustomerEmail;

                if (!string.IsNullOrEmpty(orderUpdateDTO.CustomerAddress))
                    existingOrder.CustomerAddress = orderUpdateDTO.CustomerAddress;


                if (!string.IsNullOrEmpty(orderUpdateDTO.Status) && existingOrder.Status != orderUpdateDTO.Status)
                {
                    existingOrder.Status = orderUpdateDTO.Status;
                    await _emailService.SendUpdateOrderEmail(existingOrder.CustomerName, existingOrder.CustomerEmail, orderUpdateDTO.Status);
                }        

                await _appDbContext.SaveChangesAsync();
                return existingOrder;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении заказа: {ex.Message}", ex);
            }
        }
    }
}
