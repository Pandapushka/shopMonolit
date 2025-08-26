using shop.Data;

namespace shop.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _appDbContext;
        public OrderService(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }
    }
}
