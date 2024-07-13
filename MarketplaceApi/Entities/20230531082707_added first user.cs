using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketplaceApi.Entities
{
    public partial class addedfirstuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Admin", "DeliveryAddress", "Email", "FirstName", "Phone", "RegistrationDate", "SecondName", "Seller" },
                values: new object[] { 1, true, "Moscow", "1111mail.com", "User1", "+71111111111", new DateTime(2023, 5, 31, 11, 27, 6, 491, DateTimeKind.Local).AddTicks(5569), "User1", false });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
