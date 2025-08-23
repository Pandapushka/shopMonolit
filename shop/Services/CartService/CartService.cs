using Microsoft.EntityFrameworkCore;
using shop.Data;
using shop.Model.Entitys.Cart;

namespace shop.Services.CartService
{
    public class CartService
    {
        private readonly AppDbContext _appDbContext;
        public CartService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task CreateNewCartAsync(string userID, int productId, int quantity)
        {
            if (string.IsNullOrWhiteSpace(userID))
                throw new ArgumentException("UserID не может быть пустым");
            if (productId <= 0)
                throw new ArgumentException("ProductId должен быть положительным числом");
            if (quantity <= 0)
                throw new ArgumentException("Quantity должен быть положительным числом");
            try 
            {
                ShoppingCart shoppingCart = new ShoppingCart
                {
                    UserID = userID
                };

                await _appDbContext.Carts.AddAsync(shoppingCart);
                await _appDbContext.SaveChangesAsync();

                CartItem cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    ShoppingCart = shoppingCart,
                    Product = null
                };

                await _appDbContext.CartItems.AddAsync(cartItem);
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Не удалось сохранить корзину в базу данных");
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла ошибка при создании корзины");
            }
        }

        public async Task UpdateExistingCartAsync(int cartId, int productId, int newQuality)
        {
            if (cartId <= 0)
                throw new ArgumentException("CartId должен быть положительным числом");
            try 
            {
                var existingCart = await _appDbContext.Carts.FirstOrDefaultAsync(c => c.Id == cartId);

                if(existingCart == null)
                    throw new ArgumentException("Данной Cart не существует");

                bool productExists = await _appDbContext.Products.AnyAsync(p => p.Id == productId);
                if (!productExists)
                    throw new ArgumentException($"Продукт с ID {productId} не существует");

                CartItem cartItemInCart = existingCart.Items.FirstOrDefault(e=>e.ProductId == productId);

                if (cartItemInCart == null && newQuality > 0)
                {
                    CartItem cartItem = new CartItem
                    {
                        ProductId = productId,
                        Quantity = newQuality,
                        ShoppingCartId = existingCart.Id
                    };

                    await _appDbContext.CartItems.AddAsync(cartItem);
                }
                else if (cartItemInCart != null)
                {
                    int updateQuantity = cartItemInCart.Quantity + newQuality;
                    if (newQuality == 0 || updateQuantity <= 0)
                    {
                        _appDbContext.CartItems.Remove(cartItemInCart);
                        if (existingCart.Items.Count == 1)
                        {
                            _appDbContext.Carts.Remove(existingCart);
                        }
                    }
                    else 
                    {
                        cartItemInCart.Quantity = newQuality;
                    }
                }
                await _appDbContext.SaveChangesAsync();


            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Ошибка при обновлении базы данных");
            }

            catch (InvalidOperationException ex)
            {
                throw new Exception("Ошибка при работе с элементами корзины");
            }
        }
    }
}
