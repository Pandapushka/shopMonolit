using shop.Model.Entitys.Cart;
using System.ComponentModel.DataAnnotations;

namespace shop.Model.Entitys
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public string SpecialTag { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        [Required]
        [Range(1, 200000)]
        public double Price { get; set; }
        [Required]
        public string Image { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
