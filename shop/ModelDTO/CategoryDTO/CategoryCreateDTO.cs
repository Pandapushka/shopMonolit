﻿using System.ComponentModel.DataAnnotations;

namespace shop.ModelDTO.CategoryDTO
{
    public class CategoryCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }
    }
}
