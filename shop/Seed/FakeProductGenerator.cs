using Bogus;
using shop.Model.Entitys;

namespace shop.Seed
{
    public static class FakeProductGenerator
    {

        public static Category GenerateCategory()
        {
            return new Category
            {
                Id = 1,
                Name = "Foo",
                Description = "Bar",
                ImageUrl = "https://s3.twcstorage.ru/2d0ce14a-f23b89b9-0eff-4dd9-9032-96409551c4d9/12121212_77cda043fc174098b88746853a31adaa.PNG"
            };
        }
        public static List<Product> GenerateProductList(int count = 5)
        {
            var specialTag = new List<string> { "Тег 1", "Тег 2", "Тег 3" };

            return new Faker<Product>("ru")
                .RuleFor(m => m.Id, f => f.IndexFaker + 1)
                .RuleFor(m => m.Image, f => $"https://placehold.net/400x400.png")
                .RuleFor(m=>m.Name, f=>f.Commerce.ProductName())
                .RuleFor(m=>m.Description, f=>f.Lorem.Sentence())
                .RuleFor(m=>m.CategoryId, f=> 1)
                .RuleFor(m=>m.SpecialTag, f=>f.PickRandom(specialTag))
                .RuleFor(m=>m.Price, f=>Math.Round(f.Random.Double(1,5000), 2))
                .Generate(count);
        }
    }
}
