using shop.Model.Entitys.Order;
using shop.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shop.ModelDTO.OrderDTO
{
    public class OrderUpdateDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
