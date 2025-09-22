using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace shop.Extension
{
    public static class AuthenticationServiceExtension
    {
        public static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var authSettingsToken = configuration["AuthSettings:SecretKey"];
            if (string.IsNullOrEmpty(authSettingsToken))
                throw new InvalidOperationException("AuthSettings:SecretKey is not configured.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(options =>
               {
                   options.RequireHttpsMetadata = false; // Для development
                   options.SaveToken = true;

                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettingsToken)),
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       ClockSkew = TimeSpan.Zero,
                       NameClaimType = ClaimTypes.NameIdentifier,
                       RoleClaimType = ClaimTypes.Role
                   };

                  
               });

            return services;
        }
    }
}
