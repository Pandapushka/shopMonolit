using shop.Services.OrderService;

namespace shop.Controllers
{
    public class OrderController : StoreController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
    }
}
