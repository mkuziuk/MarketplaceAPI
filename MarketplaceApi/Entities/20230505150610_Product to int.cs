using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketplaceApi.Entities
{
    public partial class Producttoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("WhereUsed", "Product");
            migrationBuilder.AddColumn<int>(
                name: "WhereUsed",
                table: "Product",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.DropColumn("UseCase", "Product");
            migrationBuilder.AddColumn<int>(
                name: "UseCase",
                table: "Product",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.DropColumn("Type", "Product");
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Product",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.DropColumn("Material", "Product");
            migrationBuilder.AddColumn<int>(
                name: "Material",
                table: "Product",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WhereUsed",
                table: "Product",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "UseCase",
                table: "Product",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Product",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Material",
                table: "Product",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
