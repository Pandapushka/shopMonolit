using Microsoft.Extensions.DependencyInjection;
using shop.Services.JwtService;

namespace shop.Extension
{
    public static class JwtTokenGeneratorServiceExtension
    {
        public static IServiceCollection AddJwtTokenGenerator(this IServiceCollection services)
        {
            return services.AddScoped<JwtTokenGenerator>();
        }
    }
}
