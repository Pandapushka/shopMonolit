using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Image", "Name", "Price", "SpecialTag" },
                values: new object[,]
                {
                    { 1, "Категория 3", "Обеспечивает повседневная специалистов современного.", "https://placehold.net/400x400.png", "Практичный Резиновый Клатч", 536.28999999999996, "Тег 3" },
                    { 2, "Категория 2", "За начало зависит забывать.", "https://placehold.net/400x400.png", "Фантастический Хлопковый Свитер", 4000.2800000000002, "Тег 3" },
                    { 3, "Категория 1", "Качественно кадров интересный кадровой путь анализа структура постоянный дальнейшее этих.", "https://placehold.net/400x400.png", "Великолепный Гранитный Куртка", 2247.2399999999998, "Тег 2" },
                    { 4, "Категория 2", "Шагов высшего материально-технической богатый сущности прогрессивного показывает намеченных кадровой начало.", "https://placehold.net/400x400.png", "Свободный Гранитный Ремень", 3207.2199999999998, "Тег 2" },
                    { 5, "Категория 2", "Участниками шагов степени укрепления.", "https://placehold.net/400x400.png", "Фантастический Гранитный Свитер", 310.06999999999999, "Тег 2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
