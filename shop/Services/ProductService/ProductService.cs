using Bogus;
using Microsoft.EntityFrameworkCore;
using shop.Data;
using shop.Mappers;
using shop.Model.Entitys;
using shop.ModelDTO;
using shop.Services.Storage;

namespace shop.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IFileStorageService _fileStorageService;
        public ProductService(AppDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("ID должен быть положительным");
                if(!await IsProductNotDeleted(id))
                    throw new ArgumentException("Продукт с таким id не существует");


                var product = await _context.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (product == null)
                    throw new ArgumentException("ID должен быть валидным");
                return product;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task Create(ProductCreateDTO productCreateDTO)
        {
            try
            {
                if (productCreateDTO == null)
                    throw new ArgumentNullException("Пришла пустая модель продукта");

                var categoryExists = await _context.Categories
                    .AnyAsync(c => c.Id == productCreateDTO.CategoryId && c.IsActive);

                if (!categoryExists)
                    throw new ArgumentException("Указанная категория не существует или неактивна");

                var product = ProductMapper.ToProduct(productCreateDTO, await _fileStorageService.UploadFileAsync(productCreateDTO.Image));

                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public async Task Update(int id, ProductCreateDTO productCreateDTO)
        {
            try
            {
                if (productCreateDTO == null)
                    throw new ArgumentNullException("Не валидная модель с фронта");

                if (id <= 0)
                    throw new ArgumentException("Некорректный Id");

                var productFromDb = await GetById(id);

                if (productFromDb == null)
                    throw new ArgumentException("Продукт не существует");

                if(!String.IsNullOrEmpty(productCreateDTO.Name))
                    productFromDb.Name = productCreateDTO.Name;

                if (!String.IsNullOrEmpty(productCreateDTO.Description))
                    productFromDb.Description = productCreateDTO.Description;

                if (productCreateDTO.CategoryId != 0 && productCreateDTO.CategoryId != productFromDb.CategoryId)
                {
                    var categoryExists = await _context.Categories
                        .AnyAsync(c => c.Id == productCreateDTO.CategoryId && c.IsActive);

                    if (!categoryExists)
                        throw new ArgumentException("Указанная категория не существует или неактивна");

                    productFromDb.CategoryId = productCreateDTO.CategoryId;
                }

                if (!String.IsNullOrEmpty(productCreateDTO.SpecialTag))
                    productFromDb.SpecialTag = productCreateDTO.SpecialTag;

                if(productCreateDTO.Price != 0)
                    productFromDb.Price = productCreateDTO.Price;

                if ((productCreateDTO.Image != null))
                    productFromDb.Image = await _fileStorageService.UploadFileAsync(productCreateDTO.Image);

                _context.Products.Update(productFromDb);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Некорректный Id");

            var productFromDb = await GetById(id);

            if (productFromDb == null)
                throw new ArgumentException("Продукт не существует");

            productFromDb.IsDeleted = true;
            _context.Products.Update(productFromDb);
            await _context.SaveChangesAsync();

        }

        private async Task<bool> IsProductNotDeleted(int id)
        {
            try
            {
                if (id <= 0)
                    return false;

                return await _context.Products
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .AnyAsync(x => !x.IsDeleted);
            }
            catch (Exception ex)
            {
                throw; 
            }
        }
    }
}
