using shop.Extension;
using shop.Services.AuthService;
using shop.Services.ProductService;

namespace shop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGenCustomConfig();
            builder.Services.AddPostgreSqlDbContext(builder.Configuration);
            builder.Services.AddPostgreSqlIdentityContext();
            builder.Services.AddAdminInitializer();
            builder.Services.AddConfigureIdentityOptions();
            builder.Services.AddJwtTokenGenerator();
            builder.Services.AddCors();


            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            await app.Services.InitializeRoleAsync();

            using (var scope = app.Services.CreateScope())
            {
                var adminService = scope.ServiceProvider.GetRequiredService<AdminInitializerService>();
                await adminService.InitializeAdminAsync();
            }
            app.UseCors(o=>
                o.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .WithExposedHeaders("*")
                );


            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
