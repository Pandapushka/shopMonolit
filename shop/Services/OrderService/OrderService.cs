using Microsoft.EntityFrameworkCore;
using shop.Common;
using shop.Data;
using shop.Model.Entitys.Order;
using shop.ModelDTO.OrderDTO;
using shop.Seed;

namespace shop.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _appDbContext;
        public OrderService(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }

        public async Task<Order> CreateOrderAsync(OrderCreateDTO orderCreateDTO)
        {
            try
            {
                var order = new Order
                {
                    CustomerName = orderCreateDTO.CustomerName,
                    CustomerAddress = orderCreateDTO.CustomerAddress,
                    CustomerEmail = orderCreateDTO.CustomerEmail,
                    AppUserId = orderCreateDTO.AppUserId,
                    OrderTotalAmount = orderCreateDTO.OrderTotalAmount,
                    TotalCount = orderCreateDTO.TotalCount,
                    Status = string.IsNullOrEmpty(orderCreateDTO.Status) ? OrderStatus.Accepted : orderCreateDTO.Status,
                    OrderDateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                };

                await _appDbContext.Order.AddAsync(order);
                await _appDbContext.SaveChangesAsync();

                if (orderCreateDTO.OrderDetailItems != null)
                {
                    foreach (var orderDetailsDto in orderCreateDTO.OrderDetailItems)
                    {
                        var orderDetails = new OrderDetails
                        {
                            OrderId = order.Id,
                            ProductId = orderDetailsDto.ProductId,
                            Quantity = orderDetailsDto.Quantity,
                            ItemName = orderDetailsDto.ItemName,
                            Price = orderDetailsDto.Price
                        };

                        await _appDbContext.OrderDetails.AddAsync(orderDetails);
                    }
                    await _appDbContext.SaveChangesAsync();
                }

                return order;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при создании заказа: {ex.Message}", ex);
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

                if (!string.IsNullOrEmpty(orderUpdateDTO.Status))
                    existingOrder.Status = orderUpdateDTO.Status;

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
