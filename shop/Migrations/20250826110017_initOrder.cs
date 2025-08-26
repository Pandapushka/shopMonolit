using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop.Migrations
{
    /// <inheritdoc />
    public partial class initOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Настолько изменений соображения нами процесс.", "Маленький Резиновый Кошелек", 4021.0999999999999, "Тег 3" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 3", "Рамки новых концепция.", "Невероятный Натуральный Кошелек", 1777.8399999999999, "Тег 3" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 3", "Разработке путь уровня различных предложений разнообразный обуславливает различных ресурсосберегающих.", "Эргономичный Кожанный Берет", 4264.1300000000001, "Тег 1" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 3", "Сознания соображения условий повседневная степени напрямую важные этих создаёт деятельности.", "Лоснящийся Деревянный Плащ", 2125.1500000000001, "Тег 1" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 2", "Существующий модернизации роль существующий определения по образом активности качества.", "Фантастический Неодимовый Кошелек", 3078.2199999999998, "Тег 1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Постоянный создание таким постоянное особенности интересный.", "Интеллектуальный Хлопковый Майка", 820.66999999999996, "Тег 2" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 1", "Предпосылки различных модели значимость предпосылки создаёт не соображения.", "Большой Резиновый Свитер", 2378.9299999999998, "Тег 2" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 2", "Ресурсосберегающих соответствующей повышению и в принципов показывает задач правительством организационной.", "Лоснящийся Деревянный Куртка", 3045.1500000000001, "Тег 2" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 2", "Модернизации принципов профессионального высокотехнологичная.", "Фантастический Стальной Ножницы", 4665.5600000000004, "Тег 3" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 1", "Сомнений повышение степени модель поставленных значение однако.", "Интеллектуальный Гранитный Сабо", 1754.6700000000001, "Тег 2" });
        }
    }
}
