using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shop.Common;
using shop.Data;
using shop.Model;
using shop.ModelDTO;
using shop.Services.EmailConfirmationService;
using shop.Services.JwtService;

namespace shop.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailConfirmationService _emailConfirmationService;
        public AuthService(AppDbContext appDbContext,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager, JwtTokenGenerator jwtTokenGenerator, IEmailConfirmationService emailConfirmationService)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailConfirmationService = emailConfirmationService;
        }
        
        public async Task Register(RegisterRequestDTO registerRequestDTO)
        {
            try
            {
                if (registerRequestDTO == null)
                    throw new ArgumentNullException("Пришла пустая модель регистрации");

                var userFromDb = await _appDbContext.AppUsers.FirstOrDefaultAsync(
                    u => u.Email.ToLower() == registerRequestDTO.Email.ToLower());

                if (userFromDb != null)
                    throw new ArgumentNullException("Польлзователь с таким Email уже существует");

                if (registerRequestDTO.Password != registerRequestDTO.PasswordReapeat)
                    throw new ArgumentNullException("Введенные пароли не совпадают");

                var newAppUser = new AppUser
                {
                    Email = registerRequestDTO.Email,
                    UserName = registerRequestDTO.UserName,
                    NormalizedEmail = registerRequestDTO.Email.ToUpper(),
                    EmailConfirmed = false
                };

                var result = await _userManager.CreateAsync(newAppUser, registerRequestDTO.Password);

                var code = await _emailConfirmationService.GenerateConfirmationCodeAsync(registerRequestDTO.Email);
                await _emailConfirmationService.SendConfirmationCodeAsync(registerRequestDTO.Email, code);

                if (!result.Succeeded)
                    throw new ArgumentNullException("Ошибка регистрации");

                await _userManager.AddToRoleAsync(newAppUser, SharedData.Roles.Consumer);

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }


        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var userFromDb = await _appDbContext.AppUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequestDTO.Email.ToLower());

                if (userFromDb == null)
                    throw new ArgumentNullException("Такого пользователя не существует");
                if (!await _userManager.CheckPasswordAsync(userFromDb, loginRequestDTO.Password))
                    throw new ArgumentNullException("Не верный пароль");

                var roles = await _userManager.GetRolesAsync(userFromDb);
                var token = _jwtTokenGenerator.GenerateJwtToken(userFromDb, roles);

                return new LoginResponseDTO 
                {
                    Token = token,
                    Email = userFromDb.Email
                };

            }
            catch (Exception ex) 
            {
                throw new ArgumentException(ex.Message);
            }
            
        }

    }
}
