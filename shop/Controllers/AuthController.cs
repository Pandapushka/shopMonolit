using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shop.Model;
using shop.ModelDTO;
using shop.Services.AuthService;

namespace shop.Controllers
{
    public class AuthController : StoreController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ResponseServer<string>>> Register([FromBody] RegisterRequestDTO registerDTO)
        {
            try
            {
                if (registerDTO == null)
                    return BadRequest(ResponseServer<string>.Error("Модель регистрации не может быть пустой", 400));

                if (!ModelState.IsValid)
                    return BadRequest(ResponseServer<string>.Error("Некорректные данные валидации", 400));

                await _authService.Register(registerDTO);
                return Ok(ResponseServer<string>.Success("Пользователь успешно зарегистрирован", 201));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<string>.Error("Произошла ошибка при регистрации", 500));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseServer<LoginResponseDTO>>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                if (loginRequestDTO == null)
                    return BadRequest(ResponseServer<LoginResponseDTO>.Error("Модель входа не может быть пустой", 400));

                if (!ModelState.IsValid)
                    return BadRequest(ResponseServer<LoginResponseDTO>.Error("Некорректные данные валидации", 400));

                var result = await _authService.Login(loginRequestDTO);
                return Ok(ResponseServer<LoginResponseDTO>.Success(result, 200));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ResponseServer<LoginResponseDTO>.Error(ex.Message, 400));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ResponseServer<LoginResponseDTO>.Error("Произошла ошибка при входе", 500));
            }
        }

    }
}
