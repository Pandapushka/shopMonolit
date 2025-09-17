using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shop.Common;
using shop.Controllers;
using shop.Data;
using shop.Model;
using shop.Seed;

namespace shop.Services.PaymentService
{
    public class FakePaymentService : IPaymentService
    {
        private readonly AppDbContext _context;
        public FakePaymentService(AppDbContext context)
        {
            _context = context;       
        }
        public async Task<ActionResult<ResponseServer<string>>> HandlerPaymentAsync(string userId, int orderId, string cardNumber)
        {

            var order = await _context.Order.FindAsync(orderId);
            if (order == null)
                return new BadRequestObjectResult(ResponseServer<string>.Error("Произошла ошибка при получении данных о заказе"));

            PaymentResponse response;
            if (cardNumber == "1111 2222 3333 4444")
            {
                response = new PaymentResponse
                {
                    Success = true,
                    InterntId = "Fake_Id",
                    Secret = "secret",
                };
            }
            else 
            {
                response = new PaymentResponse
                {
                    Success = false,
                    ErrorMessage = "Платеж не удался"
                };
            }
            if (!response.Success)
                return new BadRequestObjectResult(ResponseServer<string>.Error("Оплата данной картой не прошла"));

            
            order.Status = OrderStatus.Payment;
            await _context.SaveChangesAsync();

            return ResponseServer<string>.Success("Заказ оплачен", 200);
        }
    }
}
