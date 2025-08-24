using shop.Services.CartService;
using System.Runtime.CompilerServices;

namespace shop.Extension
{
    public static class CartServiceExtention
    {
        public static IServiceCollection AddCartService(
            this IServiceCollection services
            )
        {
            return services.AddScoped<ICartService, CartService>();
        }
    }
}
