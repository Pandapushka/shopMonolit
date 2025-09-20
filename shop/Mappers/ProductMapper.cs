using shop.Model.Entitys;
using shop.ModelDTO;

namespace shop.Mappers
{
    public class ProductMapper
    {
        public static Product ToProduct(ProductCreateDTO createDTO, string image)
        {
            Product product = new()
            {
                Name = createDTO.Name,
                Description = createDTO.Description,
                SpecialTag = createDTO.SpecialTag,
                Category = createDTO.Category,
                Price = createDTO.Price,
                Image = image
            };
            return product;
        }
    }
}
