using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shop.Migrations
{
    /// <inheritdoc />
    public partial class Update_Product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Carts_ShoppingCartId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Products_ProductId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Items",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "Items",
                newName: "CartItems");

            migrationBuilder.RenameIndex(
                name: "IX_Items_ShoppingCartId",
                table: "CartItems",
                newName: "IX_CartItems_ShoppingCartId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_ProductId",
                table: "CartItems",
                newName: "IX_CartItems_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "Постоянный создание таким постоянное особенности интересный.", "Интеллектуальный Хлопковый Майка", 820.66999999999996 });

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
                columns: new[] { "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Ресурсосберегающих соответствующей повышению и в принципов показывает задач правительством организационной.", "Лоснящийся Деревянный Куртка", 3045.1500000000001, "Тег 2" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { "Категория 2", "Модернизации принципов профессионального высокотехнологичная.", "Фантастический Стальной Ножницы", 4665.5600000000004 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Сомнений повышение степени модель поставленных значение однако.", "Интеллектуальный Гранитный Сабо", 1754.6700000000001, "Тег 2" });

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_ShoppingCartId",
                table: "CartItems",
                column: "ShoppingCartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_ShoppingCartId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems");

            migrationBuilder.RenameTable(
                name: "CartItems",
                newName: "Items");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_ShoppingCartId",
                table: "Items",
                newName: "IX_Items_ShoppingCartId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_ProductId",
                table: "Items",
                newName: "IX_Items_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items",
                table: "Items",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "Позволяет позволяет другой формирования оценить качества.", "Практичный Меховой Майка", 4526.0799999999999 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Категория 2", "Однако зависит степени рамки создание сомнений общественной.", "Большой Бетонный Свитер", 4567.5500000000002, "Тег 1" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Выбранный очевидна развития управление обуславливает идейные.", "Грубый Меховой Клатч", 1245.25, "Тег 1" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Description", "Name", "Price" },
                values: new object[] { "Категория 1", "Порядка зависит повседневная принципов зависит демократической сущности.", "Большой Деревянный Плащ", 1782.6500000000001 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Name", "Price", "SpecialTag" },
                values: new object[] { "Обеспечивает напрямую кадров проект и нашей существующий предложений всего новая.", "Свободный Стальной Стол", 1808.1199999999999, "Тег 1" });

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Carts_ShoppingCartId",
                table: "Items",
                column: "ShoppingCartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Products_ProductId",
                table: "Items",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
