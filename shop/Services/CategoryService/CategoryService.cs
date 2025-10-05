using Microsoft.EntityFrameworkCore;
using shop.Data;
using shop.Model.Entitys;
using shop.ModelDTO.CategoryDTO;
using shop.Services.Storage;

namespace shop.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public CategoryService(AppDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<List<CategoryResponseDTO>> GetAllCategoriesAsync(bool includeInactive = false)
        {
            var query = _context.Categories.AsQueryable();

            if (!includeInactive)
            {
                query = query.Where(c => c.IsActive);
            }

            var categories = await query
                .Include(c => c.Products)
                .Select(c => MapToDto(c))
                .ToListAsync();

            return categories;
        }

        public async Task<CategoryResponseDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                throw new ArgumentException($"Категория с ID {id} не найдена");

            return MapToDto(category);
        }

        public async Task<CategoryResponseDTO> CreateCategoryAsync(CategoryCreateDTO categoryDto)
        {

            var existingCategory = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryDto.Name.ToLower());

            if (existingCategory != null)
                throw new ArgumentException($"Категория с названием '{categoryDto.Name}' уже существует");

            string imageUrl = string.Empty;
            if (categoryDto.Image != null)
            {
                imageUrl = await _fileStorageService.UploadFileAsync(categoryDto.Image);
            }

            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                ImageUrl = imageUrl,
                IsActive = true
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return await GetCategoryByIdAsync(category.Id);
        }

        public async Task<CategoryResponseDTO> UpdateCategoryAsync(int id, CategoryUpdateDTO categoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new ArgumentException($"Категория с ID {id} не найдена");

            // Проверяем уникальность названия (если меняется)
            if (!string.IsNullOrEmpty(categoryDto.Name) && categoryDto.Name != category.Name)
            {
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryDto.Name.ToLower() && c.Id != id);

                if (existingCategory != null)
                    throw new ArgumentException($"Категория с названием '{categoryDto.Name}' уже существует");
            }

            // Обновляем поля
            if (!string.IsNullOrEmpty(categoryDto.Name))
                category.Name = categoryDto.Name;

            if (!string.IsNullOrEmpty(categoryDto.Description))
                category.Description = categoryDto.Description;

            if (categoryDto.IsActive.HasValue)
                category.IsActive = categoryDto.IsActive.Value;

            // Обновляем изображение если нужно
            if (categoryDto.Image != null)
            {
                // Удаляем старое изображение
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    await _fileStorageService.RemoveFileAsync(category.ImageUrl);
                }
                category.ImageUrl = await _fileStorageService.UploadFileAsync(categoryDto.Image);
            }

            await _context.SaveChangesAsync();
            return await GetCategoryByIdAsync(category.Id);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                throw new ArgumentException($"Категория с ID {id} не найдена");

            // Проверяем можно ли удалить категорию
            if (category.Products.Any(p => !p.IsDeleted))
                throw new InvalidOperationException("Невозможно удалить категорию, так как к ней привязаны товары");

            // Удаляем изображение
            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                await _fileStorageService.RemoveFileAsync(category.ImageUrl);
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ToggleCategoryStatusAsync(int id, bool isActive)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                throw new ArgumentException($"Категория с ID {id} не найдена");

            category.IsActive = isActive;
            await _context.SaveChangesAsync();

            return true;
        }

        private static CategoryResponseDTO MapToDto(Category category)
        {
            return new CategoryResponseDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                IsActive = category.IsActive,
                ProductsCount = category.Products.Count(p => !p.IsDeleted)
            };
        }
    }
}
