using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketplaceApi.Entities
{
    public partial class AddedOrderSatusId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderStatusId",
                table: "Order",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatusId",
                table: "Order");
        }
    }
}
