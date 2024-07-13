using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketplaceApi.Entities
{
    public partial class Addedinitialdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2023, 5, 31, 11, 43, 45, 7, DateTimeKind.Local).AddTicks(8255));

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Admin", "DeliveryAddress", "Email", "FirstName", "Phone", "RegistrationDate", "SecondName", "Seller" },
                values: new object[,]
                {
                    { 3, false, "Ufa", "3333mail.com", "User3", "+73333333333", new DateTime(2023, 5, 31, 11, 43, 45, 12, DateTimeKind.Local).AddTicks(2136), "User3", false },
                    { 2, false, "Belgorod", "2222mail.com", "User2", "+72222222222", new DateTime(2023, 5, 31, 11, 43, 45, 12, DateTimeKind.Local).AddTicks(1891), "User2", true }
                });

            migrationBuilder.InsertData(
                table: "Shop",
                columns: new[] { "Id", "CreationDate", "Name", "OwnerId" },
                values: new object[] { 1, new DateTime(2023, 5, 31, 11, 43, 45, 13, DateTimeKind.Local).AddTicks(5707), "shop1", 2 });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "Id", "CreationDate", "Height", "InStockQuantity", "IsPublic", "Length", "Material", "Name", "Price", "PublicationDate", "ShopId", "Type", "UseCase", "UserId", "WhereUsed", "Width" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 31, 11, 43, 45, 14, DateTimeKind.Local).AddTicks(379), 6, 20, true, 6, 1, "ball", 1500, new DateTime(2023, 5, 31, 11, 43, 45, 14, DateTimeKind.Local).AddTicks(816), 1, 1, 1, 2, 1, 6 },
                    { 2, new DateTime(2023, 5, 31, 11, 43, 45, 14, DateTimeKind.Local).AddTicks(1544), 16, 25, true, 5, 2, "tower", 2500, new DateTime(2023, 5, 31, 11, 43, 45, 14, DateTimeKind.Local).AddTicks(1563), 1, 2, 2, 2, 2, 5 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Shop",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2023, 5, 31, 11, 27, 6, 491, DateTimeKind.Local).AddTicks(5569));
        }
    }
}
