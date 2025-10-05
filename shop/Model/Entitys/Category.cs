using System.ComponentModel.DataAnnotations;

namespace shop.Model.Entitys
{
    public class Category : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public bool IsActive { get; set; } = true;
    }
}
