using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketplaceApi.Entities
{
    public partial class UsershavemanyShops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Shop_ShopId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ShopId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "User");

            migrationBuilder.CreateTable(
                name: "ShopUser",
                columns: table => new
                {
                    ShopOwnersIdId = table.Column<int>(type: "integer", nullable: false),
                    ShopsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopUser", x => new { x.ShopOwnersIdId, x.ShopsId });
                    table.ForeignKey(
                        name: "FK_ShopUser_Shop_ShopsId",
                        column: x => x.ShopsId,
                        principalTable: "Shop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopUser_User_ShopOwnersIdId",
                        column: x => x.ShopOwnersIdId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopUser_ShopsId",
                table: "ShopUser",
                column: "ShopsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopUser");

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "User",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_ShopId",
                table: "User",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Shop_ShopId",
                table: "User",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
