using Microsoft.AspNetCore.Identity;
using shop.Data;
using shop.Model.Entitys.Order;
using shop.Model;
using shop.ModelDTO.UserDTO;
using shop.Services.OrderService;
using Microsoft.EntityFrameworkCore;

namespace shop.Services.UserService
{
    public class UserProfileService : IUserProfileService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderService _orderService;

        public UserProfileService(
            AppDbContext context,
            UserManager<AppUser> userManager,
            IOrderService orderService)
        {
            _context = context;
            _userManager = userManager;
            _orderService = orderService;
        }

        public async Task<UserProfileDTO> GetUserProfileAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            var ordersCount = await _context.Order
                .CountAsync(o => o.AppUserId == userId);

            return new UserProfileDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                UserLastName = user.UserLastName,
                EmailConfirmed = user.EmailConfirmed,
                OrdersCount = ordersCount
            };
        }

        public async Task<List<UserOrderDTO>> GetUserOrdersAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required");

            var orders = await _context.Order
                .Where(o => o.AppUserId == userId)
                .Include(o => o.OrderDetailItems)
                    .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.OrderDateTime)
                .ToListAsync();

            return orders.Select(MapToUserOrderDTO).ToList();
        }

        public async Task<UserOrderDTO> GetUserOrderByIdAsync(string userId, int orderId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required");

            var order = await _context.Order
                .Include(o => o.OrderDetailItems)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.AppUserId == userId);

            if (order == null)
                throw new ArgumentException($"Order with ID {orderId} not found or access denied");

            return MapToUserOrderDTO(order);
        }

        public async Task<bool> UpdateUserProfileAsync(string userId, UpdateUserProfileDTO updateDto)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID is required");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            // Обновляем поля
            if (!string.IsNullOrEmpty(updateDto.UserName))
                user.UserName = updateDto.UserName;

            if (!string.IsNullOrEmpty(updateDto.UserLastName))
                user.UserLastName = updateDto.UserLastName;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<List<UserProfileDTO>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var userProfiles = new List<UserProfileDTO>();

            foreach (var user in users)
            {
                var ordersCount = _context.Order
                    .Count(o => o.AppUserId == user.Id);

                userProfiles.Add(new UserProfileDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    UserLastName = user.UserLastName,
                    EmailConfirmed = user.EmailConfirmed,
                    OrdersCount = ordersCount
                });
            }

            return userProfiles;
        }

        private UserOrderDTO MapToUserOrderDTO(Order order)
        {
            return new UserOrderDTO
            {
                Id = order.Id,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                CustomerAddress = order.CustomerAddress,
                OrderTotalAmount = order.OrderTotalAmount,
                OrderDateTime = order.OrderDateTime,
                Status = order.Status,
                TotalCount = order.TotalCount,
                OrderDetails = order.OrderDetailItems.Select(od => new UserOrderDetailDTO
                {
                    ItemName = od.ItemName,
                    Quantity = od.Quantity,
                    Price = od.Price,
                    ProductImage = od.Product?.Image ?? string.Empty
                }).ToList()
            };
        }
    }
}
