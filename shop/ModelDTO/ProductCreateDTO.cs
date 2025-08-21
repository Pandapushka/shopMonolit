using System.ComponentModel.DataAnnotations;

namespace shop.ModelDTO
{
    public class ProductCreateDTO
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
    }
}
