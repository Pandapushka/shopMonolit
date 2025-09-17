using shop.Services.OrderService;
using shop.Services.PaymentService;

namespace shop.Extension
{
    public static class PaymentServiceExtension
    {
        public static IServiceCollection AddPaymentService(
           this IServiceCollection services
           )
        {
            return services.AddScoped<IPaymentService, FakePaymentService>();
        }
    }
}
