using shop.Model.Entitys;
using shop.ModelDTO;

namespace shop.Mappers
{
    public class ProductMapper
    {
        public static Product ToProduct(ProductCreateDTO createDTO)
        {
            Product product = new()
            {
                Name = createDTO.Name,
                Description = createDTO.Description,
                SpecialTag = createDTO.SpecialTag,
                Category = createDTO.Category,
                Price = createDTO.Price,
                Image = createDTO.Image
            };
            return product;
        }
    }
}
