using shop.Model.Entitys.Order;
using shop.ModelDTO.OrderDTO;

namespace shop.Services.OrderService
{
    public interface IOrderService
    {
        Task<Order> CreateOrderFromCartAsync(OrderCreateFromCartDTO orderDto);
        Task<Order> GetById(int id);
        Task<List<Order>> GetByUserId(string id);
        Task<Order> UpdateOrderAsync(int id, OrderUpdateDTO orderUpdateDTO);

    }
}
