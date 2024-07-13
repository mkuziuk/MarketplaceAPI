using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketplaceApi.Entities
{
    public partial class firstorderwithspecialstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "OrderDate", "OrderStatusId" },
                values: new object[] { new DateTime(2023, 5, 31, 16, 34, 46, 475, DateTimeKind.Local).AddTicks(3638), 99 });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDate", "PublicationDate" },
                values: new object[] { new DateTime(2023, 5, 31, 16, 34, 46, 475, DateTimeKind.Local).AddTicks(1277), new DateTime(2023, 5, 31, 16, 34, 46, 475, DateTimeKind.Local).AddTicks(1719) });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationDate", "PublicationDate" },
                values: new object[] { new DateTime(2023, 5, 31, 16, 34, 46, 475, DateTimeKind.Local).AddTicks(2468), new DateTime(2023, 5, 31, 16, 34, 46, 475, DateTimeKind.Local).AddTicks(2488) });

            migrationBuilder.UpdateData(
                table: "Shop",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2023, 5, 31, 16, 34, 46, 474, DateTimeKind.Local).AddTicks(4612));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2023, 5, 31, 16, 34, 46, 467, DateTimeKind.Local).AddTicks(8412));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegistrationDate",
                value: new DateTime(2023, 5, 31, 16, 34, 46, 472, DateTimeKind.Local).AddTicks(8588));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                column: "RegistrationDate",
                value: new DateTime(2023, 5, 31, 16, 34, 46, 472, DateTimeKind.Local).AddTicks(8924));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Order",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "OrderDate", "OrderStatusId" },
                values: new object[] { new DateTime(2023, 5, 31, 12, 30, 44, 640, DateTimeKind.Local).AddTicks(5355), 0 });

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
    }
}
