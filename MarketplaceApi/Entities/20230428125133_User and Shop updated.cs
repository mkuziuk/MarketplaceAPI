using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketplaceApi.Entities
{
    public partial class UserandShopupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopUser_Shop_ShopsId",
                table: "ShopUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopUser_User_OwnersId",
                table: "ShopUser");

            migrationBuilder.RenameColumn(
                name: "ShopsId",
                table: "ShopUser",
                newName: "ShopsWhereModeratorId");

            migrationBuilder.RenameColumn(
                name: "OwnersId",
                table: "ShopUser",
                newName: "ModeratorUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopUser_ShopsId",
                table: "ShopUser",
                newName: "IX_ShopUser_ShopsWhereModeratorId");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Shop",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shop_OwnerId",
                table: "Shop",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shop_User_OwnerId",
                table: "Shop",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopUser_Shop_ShopsWhereModeratorId",
                table: "ShopUser",
                column: "ShopsWhereModeratorId",
                principalTable: "Shop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopUser_User_ModeratorUsersId",
                table: "ShopUser",
                column: "ModeratorUsersId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shop_User_OwnerId",
                table: "Shop");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopUser_Shop_ShopsWhereModeratorId",
                table: "ShopUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopUser_User_ModeratorUsersId",
                table: "ShopUser");

            migrationBuilder.DropIndex(
                name: "IX_Shop_OwnerId",
                table: "Shop");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Shop");

            migrationBuilder.RenameColumn(
                name: "ShopsWhereModeratorId",
                table: "ShopUser",
                newName: "ShopsId");

            migrationBuilder.RenameColumn(
                name: "ModeratorUsersId",
                table: "ShopUser",
                newName: "OwnersId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopUser_ShopsWhereModeratorId",
                table: "ShopUser",
                newName: "IX_ShopUser_ShopsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopUser_Shop_ShopsId",
                table: "ShopUser",
                column: "ShopsId",
                principalTable: "Shop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopUser_User_OwnersId",
                table: "ShopUser",
                column: "OwnersId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
