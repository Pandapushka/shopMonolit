using shop.ModelDTO.UserDTO;

namespace shop.Services.UserService
{
    public interface IUserProfileService
    {
        Task<UserProfileDTO> GetUserProfileAsync(string userId);
        Task<List<UserOrderDTO>> GetUserOrdersAsync(string userId);
        Task<UserOrderDTO> GetUserOrderByIdAsync(string userId, int orderId);
        Task<bool> UpdateUserProfileAsync(string userId, UpdateUserProfileDTO updateDto);
        Task<List<UserProfileDTO>> GetAllUsersAsync();
    }
}
