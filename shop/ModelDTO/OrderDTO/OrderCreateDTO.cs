using shop.Model.Entitys.Order;
using shop.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shop.ModelDTO.OrderDTO
{
    public class OrderCreateDTO
    {
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        [Required]
        public string CustomerEmail { get; set; } = string.Empty;
        [Required]
        public string CustomerAddress { get; set; } = string.Empty;

        public string AppUserId { get; set; }
        public double OrderTotalAmount { get; set; }
        public string Status { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<OrderDetailsCreateDTO> OrderDetailItems { get; set; }

    }
}
