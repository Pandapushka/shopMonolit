using Microsoft.AspNetCore.Mvc;
using shop.Model;

namespace shop.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<ActionResult<ResponseServer<string>>> HandlerPaymentAsync(
            string UserId, int orderId, string cardNumber    
        );
    }
}
