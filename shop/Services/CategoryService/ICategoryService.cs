using shop.ModelDTO.CategoryDTO;

namespace shop.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDTO>> GetAllCategoriesAsync(bool includeInactive = false);
        Task<CategoryResponseDTO> GetCategoryByIdAsync(int id);
        Task<CategoryResponseDTO> CreateCategoryAsync(CategoryCreateDTO categoryDto);
        Task<CategoryResponseDTO> UpdateCategoryAsync(int id, CategoryUpdateDTO categoryDto);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> ToggleCategoryStatusAsync(int id, bool isActive);
    }
}
