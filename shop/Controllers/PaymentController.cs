using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using shop.Model;
using shop.Services.PaymentService;

namespace shop.Controllers
{
    public class PaymentController : StoreController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseServer<string>>> MakePayment(
            string userId, int orderId, string cardNumber)
        {
            return await _paymentService.HandlerPaymentAsync(userId, orderId, cardNumber);
        }
    }
}
