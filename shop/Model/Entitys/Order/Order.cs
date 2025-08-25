using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shop.Model.Entitys.Order
{
    public class Order : BaseEntity
    {
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public string CustomerAddress{ get; set; }

        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public double OrderTotalAmount { get; set; }

        public DateTime OrderDateTime { get; set; }
        public string Status { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<OrderDetails> OrderDetailItems { get; set; }

    }
}
