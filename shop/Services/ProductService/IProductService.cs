using shop.Model.Entitys;
using shop.ModelDTO;

namespace shop.Services.ProductService
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product?> GetById(int id);
        Task Create(ProductCreateDTO productCreateDTO);
        Task Update(int id, ProductCreateDTO productCreateDTO);
        Task Delete(int id);
    }
}
