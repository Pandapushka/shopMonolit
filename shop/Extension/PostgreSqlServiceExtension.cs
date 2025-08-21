using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shop.Data;
using shop.Model;

namespace shop.Extension
{
    public static class PostgreSqlServiceExtension
    {
        public static void AddPostgreSqlDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(
                    configuration.GetConnectionString("PostgreSQLConnection"));
            });
        }

        public static void AddPostgreSqlIdentityContext(
            this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
