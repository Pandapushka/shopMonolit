using System.ComponentModel.DataAnnotations;

namespace shop.ModelDTO.CategoryDTO
{
    public class CategoryUpdateDTO
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public IFormFile? Image { get; set; }

        public bool? IsActive { get; set; }
    }
}
