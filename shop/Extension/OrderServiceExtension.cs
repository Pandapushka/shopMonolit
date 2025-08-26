using shop.Services.CartService;
using shop.Services.OrderService;

namespace shop.Extension
{
    public static class OrderServiceExtension
    {
        public static IServiceCollection AddOrderService(
           this IServiceCollection services
           )
        {
            return services.AddScoped<IOrderService, OrderService>();
        }
    }
}
