using Microsoft.AspNetCore.Mvc;
using shop.Model;
using shop.ModelDTO;
using shop.Services.AuthService;
using shop.Services.EmailConfirmationService;

namespace shop.Controllers
{
    public class AuthController : StoreController
    {
        private readonly IAuthService _authService;
        private readonly IEmailConfirmationService _emailConfirmationService;

        public AuthController(IAuthService authService, IEmailConfirmationService emailConfirmationService)
        {
            _emailConfirmationService = emailConfirmationService;
            _authService = authService;
        }

        [HttpPost]
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

        [HttpPost]
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

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequestDTO request)
        {
            try
            {
                var isValid = await _emailConfirmationService.VerifyConfirmationCodeAsync(request.Email, request.Code);

                if (!isValid)
                    return BadRequest(ResponseServer<string>.Error("Неверный код или срок действия истек"));

                return Ok(ResponseServer<string>.Success("Email успешно подтвержден"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResendConfirmation([FromBody] ResendCodeRequestDTO request)
        {
            try
            {
                var code = await _emailConfirmationService.GenerateConfirmationCodeAsync(request.Email);
                var sent = await _emailConfirmationService.SendConfirmationCodeAsync(request.Email, code);

                if (!sent)
                    return BadRequest(ResponseServer<string>.Error("Ошибка отправки кода"));

                return Ok(ResponseServer<string>.Success("Новый код отправлен на ваш email"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseServer<string>.Error(ex.Message));
            }
        }

    }
}
