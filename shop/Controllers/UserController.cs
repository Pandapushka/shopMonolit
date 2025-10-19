using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shop.Common;
using shop.Model;
using shop.ModelDTO.UserDTO;
using shop.Services.UserService;

namespace shop.Controllers
{
    [Authorize]
    public class UserController : StoreController
    {
        private readonly IUserProfileService _userProfileService;

        public UserController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;

                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ResponseServer<UserProfileDTO>.Error("Пользователь не авторизован", 401));

                var profile = await _userProfileService.GetUserProfileAsync(userId);
                return Ok(ResponseServer<UserProfileDTO>.Success(profile));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseServer<UserProfileDTO>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<UserProfileDTO>.Error("Произошла ошибка при получении профиля", 500));
            }
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetUserOrders()
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ResponseServer<List<UserOrderDTO>>.Error("Пользователь не авторизован", 401));

                var orders = await _userProfileService.GetUserOrdersAsync(userId);
                return Ok(ResponseServer<List<UserOrderDTO>>.Success(orders));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseServer<List<UserOrderDTO>>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<List<UserOrderDTO>>.Error("Произошла ошибка при получении заказов", 500));
            }
        }

        [HttpGet("orders/{orderId:int}")]
        public async Task<IActionResult> GetUserOrderById(int orderId)
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ResponseServer<UserOrderDTO>.Error("Пользователь не авторизован", 401));

                var order = await _userProfileService.GetUserOrderByIdAsync(userId, orderId);
                return Ok(ResponseServer<UserOrderDTO>.Success(order));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ResponseServer<UserOrderDTO>.Error(ex.Message, 404));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<UserOrderDTO>.Error("Произошла ошибка при получении заказа", 500));
            }
        }

        [HttpPut("profile")]
        public async Task<ActionResult<ResponseServer<string>>> UpdateUserProfile([FromBody] UpdateUserProfileDTO updateDto)
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized(ResponseServer<string>.Error("Пользователь не авторизован", 401));

                if (updateDto == null)
                    return BadRequest(ResponseServer<string>.Error("Модель обновления не может быть пустой", 400));

                if (!ModelState.IsValid)
                    return BadRequest(ResponseServer<string>.Error("Некорректные данные валидации", 400));

                var result = await _userProfileService.UpdateUserProfileAsync(userId, updateDto);
                if (result)
                    return Ok(ResponseServer<string>.Success("Профиль успешно обновлен", 200));
                else
                    return BadRequest(ResponseServer<string>.Error("Не удалось обновить профиль", 400));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<string>.Error("Произошла ошибка при обновлении профиля", 500));
            }
        }

        [HttpGet("admin/users/{userId}")]
        [Authorize(Roles = SharedData.Roles.Admin)]
        public async Task<IActionResult> GetUserProfileById(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest(ResponseServer<UserProfileDTO>.Error("ID пользователя обязателен", 400));

                var profile = await _userProfileService.GetUserProfileAsync(userId);
                return Ok(ResponseServer<UserProfileDTO>.Success(profile));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ResponseServer<UserProfileDTO>.Error(ex.Message, 404));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<UserProfileDTO>.Error("Произошла ошибка при получении профиля пользователя", 500));
            }
        }

        [HttpGet("admin/users/{userId}/orders")]
        [Authorize(Roles = SharedData.Roles.Admin)]
        public async Task<IActionResult> GetUserOrdersByAdmin(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest(ResponseServer<List<UserOrderDTO>>.Error("ID пользователя обязателен", 400));

                var orders = await _userProfileService.GetUserOrdersAsync(userId);
                return Ok(ResponseServer<List<UserOrderDTO>>.Success(orders));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ResponseServer<List<UserOrderDTO>>.Error(ex.Message, 404));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<List<UserOrderDTO>>.Error("Произошла ошибка при получении заказов пользователя", 500));
            }
        }

        [HttpGet("admin/users")]
        [Authorize(Roles = SharedData.Roles.Admin)]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userProfileService.GetAllUsersAsync();
                return Ok(ResponseServer<List<UserProfileDTO>>.Success(users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<List<UserProfileDTO>>.Error("Произошла ошибка при получении списка пользователей", 500));
            }
        }

        // Удалите этот метод после отладки
        [HttpGet("debug-token")]
        public IActionResult DebugToken()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(claims);
        }
    }
}
