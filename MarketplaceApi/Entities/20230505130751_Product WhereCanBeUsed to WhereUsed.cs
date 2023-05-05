using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketplaceApi.Entities
{
    public partial class ProductWhereCanBeUsedtoWhereUsed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhereCanBeUsed",
                table: "Product",
                newName: "WhereUsed");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhereUsed",
                table: "Product",
                newName: "WhereCanBeUsed");
        }
    }
}
