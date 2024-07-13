using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketplaceApi.Entities
{
    public partial class Shopchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopUser_User_ShopOwnersIdId",
                table: "ShopUser");

            migrationBuilder.RenameColumn(
                name: "ShopOwnersIdId",
                table: "ShopUser",
                newName: "OwnersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopUser_User_OwnersId",
                table: "ShopUser",
                column: "OwnersId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopUser_User_OwnersId",
                table: "ShopUser");

            migrationBuilder.RenameColumn(
                name: "OwnersId",
                table: "ShopUser",
                newName: "ShopOwnersIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopUser_User_ShopOwnersIdId",
                table: "ShopUser",
                column: "ShopOwnersIdId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
