using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop.Migrations
{
    /// <inheritdoc />
    public partial class ModProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "IsDeleted", "Name", "Price" },
                values: new object[] { "Социально-ориентированный кругу обуславливает.", false, "Невероятный Резиновый Носки", 4476.6599999999999 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "IsDeleted", "Name", "Price", "SpecialTag" },
                values: new object[] { "Формирования общества инновационный особенности что деятельности предпосылки ресурсосберегающих управление позиции.", false, "Лоснящийся Кожанный Шарф", 987.54999999999995, "Тег 1" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Description", "IsDeleted", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 3", "Активом занимаемых зависит.", false, "Великолепный Резиновый Кошелек", 2192.98, "Тег 1" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Description", "IsDeleted", "Name", "Price" },
                values: new object[] { "Категория 1", "Различных отметить намеченных.", false, "Практичный Резиновый Компьютер", 1509.51 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Description", "IsDeleted", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 3", "Различных высокотехнологичная правительством значимость прогрессивного путь сфера.", false, "Лоснящийся Меховой Кошелек", 4320.04, "Тег 3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "Обеспечивает повседневная специалистов современного.", "Практичный Резиновый Клатч", 536.28999999999996 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "За начало зависит забывать.", "Фантастический Хлопковый Свитер", 4000.2800000000002, "Тег 3" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 1", "Качественно кадров интересный кадровой путь анализа структура постоянный дальнейшее этих.", "Великолепный Гранитный Куртка", 2247.2399999999998, "Тег 2" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { "Категория 2", "Шагов высшего материально-технической богатый сущности прогрессивного показывает намеченных кадровой начало.", "Свободный Гранитный Ремень", 3207.2199999999998 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 2", "Участниками шагов степени укрепления.", "Фантастический Гранитный Свитер", 310.06999999999999, "Тег 2" });
        }
    }
}
