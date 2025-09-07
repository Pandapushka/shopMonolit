using System.ComponentModel.DataAnnotations;

namespace shop.ModelDTO.OrderDTO
{
    public class OrderDetailsCreateDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string ItemName { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }

    }
}
