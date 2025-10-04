using Microsoft.AspNetCore.Mvc;
using shop.Model;
using shop.Model.Entitys.Cart;
using shop.Services.CartService;
using System.Net;

namespace shop.Controllers
{
    public class CartController : StoreController
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            try
            {
                var cart = await _cartService.GetByUserId(userId);

                if (cart == null || cart.Id == 0)
                {
                    return Ok(ResponseServer<ShoppingCart>.Success(
                        new ShoppingCart { UserID = userId }, (int)HttpStatusCode.OK));
                }

                return Ok(ResponseServer<ShoppingCart>.Success(cart, (int)HttpStatusCode.OK));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseServer<ShoppingCart>.Error(ex.Message, (int)HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseServer<ShoppingCart>.Error("Произошла ошибка при получении корзины", (int)HttpStatusCode.BadRequest));
            }
        }

        [HttpPost("{userId}/items")]
        public async Task<ActionResult<ResponseServer<string>>> AddItem(string userId, [FromQuery] int productId, [FromQuery] int quantity)
        {
            try
            {
                await _cartService.AddItemToCartAsync(userId, productId, quantity);
                return Ok(ResponseServer<string>.Success("Товар успешно добавлен в корзину", (int)HttpStatusCode.OK));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message, (int)HttpStatusCode.BadRequest));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseServer<string>.Error(ex.Message, (int)HttpStatusCode.InternalServerError));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseServer<string>.Error("Произошла ошибка при добавлении товара в корзину", (int)HttpStatusCode.InternalServerError));
            }
        }

        [HttpPut("{userId}/items/{productId}")]
        public async Task<ActionResult<ResponseServer<string>>> UpdateItem(string userId, int productId, [FromQuery] int quantity)
        {
            var i = 0;
            try
            {
                await _cartService.UpdateExistingCartAsync(userId, productId, quantity);
                return Ok(ResponseServer<string>.Success("Корзина успешно обновлена", (int)HttpStatusCode.OK));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message, (int)HttpStatusCode.BadRequest));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseServer<string>.Error(ex.Message, (int)HttpStatusCode.InternalServerError));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseServer<string>.Error("Произошла ошибка при обновлении корзины", (int)HttpStatusCode.InternalServerError));
            }
        }

        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult<ResponseServer<string>>> RemoveItem(string userId, int productId)
        {
            try
            {
                await _cartService.UpdateExistingCartAsync(userId, productId, 0);
                return Ok(ResponseServer<string>.Success("Товар успешно удален из корзины", (int)HttpStatusCode.OK));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message, (int)HttpStatusCode.BadRequest));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseServer<string>.Error(ex.Message, (int)HttpStatusCode.InternalServerError));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseServer<string>.Error("Произошла ошибка при удалении товара из корзины", (int)HttpStatusCode.InternalServerError));
            }
        }

        [HttpDelete("{userId}/items")]
        public async Task<ActionResult<ResponseServer<string>>> ClearCart(string userId)
        {
            try
            {
                await _cartService.ClearCartAsync(userId);
                return Ok(ResponseServer<string>.Success("Корзина успешно очищена", (int)HttpStatusCode.OK));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message, (int)HttpStatusCode.BadRequest));
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseServer<string>.Error(ex.Message, (int)HttpStatusCode.InternalServerError));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    ResponseServer<string>.Error("Произошла ошибка при очистке корзины", (int)HttpStatusCode.InternalServerError));
            }
        }


    }
}