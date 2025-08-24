using Microsoft.EntityFrameworkCore;
using shop.Data;
using shop.Model.Entitys.Cart;

namespace shop.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _appDbContext;
        public CartService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddItemToCartAsync(string userID, int productId, int quantity)
        {
            if (string.IsNullOrWhiteSpace(userID))
                throw new ArgumentException("UserID не может быть пустым");
            if (productId <= 0)
                throw new ArgumentException("ProductId должен быть положительным числом");
            if (quantity <= 0)
                throw new ArgumentException("Quantity должен быть положительным числом");

            try
            {
                var existingCart = await _appDbContext.Carts
                    .FirstOrDefaultAsync(c => c.UserID == userID);

                bool isNewCart = false;

                if (existingCart == null)
                {
                    existingCart = new ShoppingCart
                    {
                        UserID = userID
                    };
                    isNewCart = true;
                }

                if (isNewCart)
                {
                    await _appDbContext.Carts.AddAsync(existingCart);
                    await _appDbContext.SaveChangesAsync();
                }

                var existingCartItem = await _appDbContext.CartItems
                    .FirstOrDefaultAsync(ci => ci.ShoppingCartId == existingCart.Id && ci.ProductId == productId);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += quantity;
                }
                else
                {
                    CartItem cartItem = new CartItem
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        ShoppingCartId = existingCart.Id,
                        Product = null
                    };
                    await _appDbContext.CartItems.AddAsync(cartItem);
                }

                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Не удалось сохранить корзину в базу данных");
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла ошибка при добавлении товара в корзину");
            }
        }


        public async Task UpdateExistingCartAsync(string userId, int productId, int newQuality)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("userId передан не верно");
            try 
            {
                var existingCart = await _appDbContext.Carts.Include(u => u.Items).FirstOrDefaultAsync(c => c.UserID == userId);

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
                        cartItemInCart.Quantity = updateQuantity;
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

        public async Task ClearCartAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserID не может быть пустым", nameof(userId));

            try
            {
                var existingCart = await _appDbContext.Carts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UserID == userId);

                if (existingCart == null)
                    throw new ArgumentException("Корзина не найдена");

                // Удаляем только элементы корзины, сохраняем саму корзину
                _appDbContext.CartItems.RemoveRange(existingCart.Items);

                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Ошибка при очистке корзины в базе данных", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла ошибка при очистке корзины", ex);
            }
        }

        public async Task<ShoppingCart> GetByUserId(string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                return new ShoppingCart();
            }
            try
            {
                ShoppingCart shoppingCart = await _appDbContext.Carts
                .Include(u => u.Items)
                .ThenInclude(u => u.Product)
                .FirstOrDefaultAsync(u => u.UserID == userID);

                if (shoppingCart != null && shoppingCart.Items != null)
                {
                    shoppingCart.TotalAmount = shoppingCart.Items.Sum(u => u.Quantity * u.Product.Price);
                }

                return shoppingCart;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
