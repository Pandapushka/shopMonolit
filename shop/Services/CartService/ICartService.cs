using shop.Model.Entitys.Cart;

namespace shop.Services.CartService
{
    public interface ICartService
    {
        Task AddItemToCartAsync(string userID, int productId, int quantity);
        Task UpdateExistingCartAsync(string userID, int productId, int newQuality);
        Task<ShoppingCart> GetByUserId(string userID);
        Task ClearCartAsync(string userId);

    }
}
