using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shop.Common;
using shop.Data;
using shop.Model;
using shop.ModelDTO;

namespace shop.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        public AuthService(AppDbContext appDbContext,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
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
                    NormalizedEmail = registerRequestDTO.Email.ToUpper()
                };

                var result = await _userManager.CreateAsync(newAppUser, registerRequestDTO.Password);

                if (!result.Succeeded)
                    throw new ArgumentNullException("Ошибка регистрации");

                await _userManager.AddToRoleAsync(newAppUser, SharedData.Roles.Consumer);

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
