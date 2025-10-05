using Microsoft.AspNetCore.Authentication;
using shop.Extension;
using shop.Model.Email;
using shop.Services.AuthService;
using shop.Services.CategoryService;
using shop.Services.EmailConfirmationService;
using shop.Services.EmailService;
using shop.Services.ProductService;

namespace shop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGenCustomConfig();

            builder.Services.AddPostgreSqlDbContext(builder.Configuration);
            builder.Services.AddPostgreSqlIdentityContext();
            builder.Services.AddAdminInitializer();
            builder.Services.AddConfigureIdentityOptions();
            builder.Services.AddJwtTokenGenerator();

            builder.Services.AddAuthenticationConfig(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .WithExposedHeaders("*");
                });
            });

            builder.Services.AddFileStorageService(builder.Configuration);

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddCartService();
            builder.Services.AddOrderService();
            builder.Services.AddPaymentService();

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

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}