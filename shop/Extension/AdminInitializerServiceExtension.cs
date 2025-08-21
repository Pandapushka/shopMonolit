using Microsoft.AspNetCore.Identity;
using shop.Common;
using shop.Model;

namespace shop.Extension
{
    public static class AdminInitializerServiceExtension
    {
        public static IServiceCollection AddAdminInitializer(this IServiceCollection services)
        {
            services.AddScoped<AdminInitializerService>();
            return services;
        }
    }

    public class AdminInitializerService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AdminInitializerService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task InitializeAdminAsync()
        {
            var adminName = _configuration["Admin:AdminName"];
            var adminPassword = _configuration["Admin:AdminPassword"];

            if (string.IsNullOrEmpty(adminName) || string.IsNullOrEmpty(adminPassword))
                return;

            var existingUser = await _userManager.FindByNameAsync(adminName);
            if (existingUser == null)
            {
                var newAppUser = new AppUser
                {
                    UserName = adminName,
                    Email = adminName 
                };

                var result = await _userManager.CreateAsync(newAppUser, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newAppUser, SharedData.Roles.Admin);
                }
            }
        }
    }
}