using Bogus;
using shop.Model.Entitys;

namespace shop.Seed
{
    public static class FakeProductGenerator
    {
        public static List<Product> GenerateProductList(int count = 5)
        {
            var categories = new List<string> { "Категория 1", "Категория 2", "Категория 3" };
            var specialTag = new List<string> { "Тег 1", "Тег 2", "Тег 3" };

            return new Faker<Product>("ru")
                .RuleFor(m => m.Id, f => f.IndexFaker + 1)
                .RuleFor(m => m.Image, f => $"https://placehold.net/400x400.png")
                .RuleFor(m=>m.Name, f=>f.Commerce.ProductName())
                .RuleFor(m=>m.Description, f=>f.Lorem.Sentence())
                .RuleFor(m=>m.Category, f=>f.PickRandom(categories))
                .RuleFor(m=>m.SpecialTag, f=>f.PickRandom(specialTag))
                .RuleFor(m=>m.Price, f=>Math.Round(f.Random.Double(1,5000), 2))
                .Generate(count);
        }
    }
}
