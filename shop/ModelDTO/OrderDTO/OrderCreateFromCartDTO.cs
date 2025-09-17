using System.ComponentModel.DataAnnotations;

namespace shop.ModelDTO.OrderDTO
{
    public class OrderCreateFromCartDTO
    {
        public string CustomerName { get; set; }

        [Required]
        public string CustomerEmail { get; set; }

        [Required]
        public string CustomerAddress { get; set; }

        [Required]
        public string UserID { get; set; } 
    }
}
