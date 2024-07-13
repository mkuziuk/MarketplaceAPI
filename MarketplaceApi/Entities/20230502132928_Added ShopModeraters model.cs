using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketplaceApi.Entities
{
    public partial class AddedShopModeratersmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopUser");

            migrationBuilder.CreateTable(
                name: "ShopModerator",
                columns: table => new
                {
                    ShopId = table.Column<int>(type: "integer", nullable: false),
                    ModeratorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopModerator", x => new { x.ModeratorId, x.ShopId });
                    table.ForeignKey(
                        name: "FK_ShopModerator_Shop_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopModerator_User_ModeratorId",
                        column: x => x.ModeratorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopModerator_ShopId",
                table: "ShopModerator",
                column: "ShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopModerator");

            migrationBuilder.CreateTable(
                name: "ShopUser",
                columns: table => new
                {
                    ModeratorUsersId = table.Column<int>(type: "integer", nullable: false),
                    ShopsWhereModeratorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopUser", x => new { x.ModeratorUsersId, x.ShopsWhereModeratorId });
                    table.ForeignKey(
                        name: "FK_ShopUser_Shop_ShopsWhereModeratorId",
                        column: x => x.ShopsWhereModeratorId,
                        principalTable: "Shop",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopUser_User_ModeratorUsersId",
                        column: x => x.ModeratorUsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopUser_ShopsWhereModeratorId",
                table: "ShopUser",
                column: "ShopsWhereModeratorId");
        }
    }
}
