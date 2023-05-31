using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketplaceApi.Entities
{
    public partial class addedfirstorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Order",
                columns: new[] { "Id", "DeliveryAddress", "DeliveryDate", "OrderDate", "OrderStatusId", "SellDate", "UserId", "WayOfPayment" },
                values: new object[] { 1, null, null, new DateTime(2023, 5, 31, 12, 30, 44, 640, DateTimeKind.Local).AddTicks(5355), 0, null, 3, 0 });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDate", "PublicationDate" },
                values: new object[] { new DateTime(2023, 5, 31, 12, 30, 44, 640, DateTimeKind.Local).AddTicks(1895), new DateTime(2023, 5, 31, 12, 30, 44, 640, DateTimeKind.Local).AddTicks(2432) });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationDate", "PublicationDate" },
                values: new object[] { new DateTime(2023, 5, 31, 12, 30, 44, 640, DateTimeKind.Local).AddTicks(3612), new DateTime(2023, 5, 31, 12, 30, 44, 640, DateTimeKind.Local).AddTicks(3638) });

            migrationBuilder.UpdateData(
                table: "Shop",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2023, 5, 31, 12, 30, 44, 639, DateTimeKind.Local).AddTicks(4893));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2023, 5, 31, 12, 30, 44, 632, DateTimeKind.Local).AddTicks(848));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegistrationDate",
                value: new DateTime(2023, 5, 31, 12, 30, 44, 637, DateTimeKind.Local).AddTicks(7426));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                column: "RegistrationDate",
                value: new DateTime(2023, 5, 31, 12, 30, 44, 637, DateTimeKind.Local).AddTicks(7749));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDate", "PublicationDate" },
                values: new object[] { new DateTime(2023, 5, 31, 12, 26, 18, 718, DateTimeKind.Local).AddTicks(8239), new DateTime(2023, 5, 31, 12, 26, 18, 718, DateTimeKind.Local).AddTicks(8778) });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationDate", "PublicationDate" },
                values: new object[] { new DateTime(2023, 5, 31, 12, 26, 18, 718, DateTimeKind.Local).AddTicks(9550), new DateTime(2023, 5, 31, 12, 26, 18, 718, DateTimeKind.Local).AddTicks(9575) });

            migrationBuilder.UpdateData(
                table: "Shop",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2023, 5, 31, 12, 26, 18, 718, DateTimeKind.Local).AddTicks(1801));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2023, 5, 31, 12, 26, 18, 711, DateTimeKind.Local).AddTicks(7232));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegistrationDate",
                value: new DateTime(2023, 5, 31, 12, 26, 18, 716, DateTimeKind.Local).AddTicks(6852));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                column: "RegistrationDate",
                value: new DateTime(2023, 5, 31, 12, 26, 18, 716, DateTimeKind.Local).AddTicks(7112));
        }
    }
}
