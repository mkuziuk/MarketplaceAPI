using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MarketplaceApi.Entities
{
    public partial class addeduser2asModeratortoshop1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "ShopModerator",
                columns: new[] { "ModeratorId", "ShopId" },
                values: new object[] { 2, 1 });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ShopModerator",
                keyColumns: new[] { "ModeratorId", "ShopId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDate", "PublicationDate" },
                values: new object[] { new DateTime(2023, 5, 31, 11, 43, 45, 14, DateTimeKind.Local).AddTicks(379), new DateTime(2023, 5, 31, 11, 43, 45, 14, DateTimeKind.Local).AddTicks(816) });

            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationDate", "PublicationDate" },
                values: new object[] { new DateTime(2023, 5, 31, 11, 43, 45, 14, DateTimeKind.Local).AddTicks(1544), new DateTime(2023, 5, 31, 11, 43, 45, 14, DateTimeKind.Local).AddTicks(1563) });

            migrationBuilder.UpdateData(
                table: "Shop",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2023, 5, 31, 11, 43, 45, 13, DateTimeKind.Local).AddTicks(5707));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "RegistrationDate",
                value: new DateTime(2023, 5, 31, 11, 43, 45, 7, DateTimeKind.Local).AddTicks(8255));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "RegistrationDate",
                value: new DateTime(2023, 5, 31, 11, 43, 45, 12, DateTimeKind.Local).AddTicks(1891));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3,
                column: "RegistrationDate",
                value: new DateTime(2023, 5, 31, 11, 43, 45, 12, DateTimeKind.Local).AddTicks(2136));
        }
    }
}
