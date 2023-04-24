using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketplaceApi.Entities
{
    public partial class WithProductQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedProduct_Product_ProductId",
                table: "OrderedProduct");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "OrderedProduct",
                newName: "Quantity");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrderedProduct",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedProduct_Product_ProductId",
                table: "OrderedProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedProduct_Product_ProductId",
                table: "OrderedProduct");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderedProduct",
                newName: "ProductsId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrderedProduct",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedProduct_Product_ProductId",
                table: "OrderedProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
