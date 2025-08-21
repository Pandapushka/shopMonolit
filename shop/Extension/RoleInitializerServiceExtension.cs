using Microsoft.AspNetCore.Identity;
using shop.Common;

namespace shop.Extension
{
    public static class RoleInitializerServiceExtension
    {
        public static async Task InitializeRoleAsync(
            this IServiceProvider serviceProvider
            )
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope
                .ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in SharedData.Roles.AllRoles) 
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
