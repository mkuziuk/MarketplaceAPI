using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketplaceApi.Entities
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhereUsed",
                table: "Product",
                newName: "WhereCanBeUsed");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhereCanBeUsed",
                table: "Product",
                newName: "WhereUsed");
        }
    }
}
