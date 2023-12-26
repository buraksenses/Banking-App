using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class TransactionTablev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7a50b089-4873-4492-9810-536c23f5357a"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("99830320-0fde-4ab5-8dfd-feb5ff4a21ab"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("9be1c6d7-7f24-4c3b-bbf7-4cc42a60744c"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a0100f94-ad41-4128-95f8-168fb679bb93"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a60709d5-9612-4314-b47b-0d63fcda119a"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cae19779-86af-490a-9def-bbc17b26371c"));

            migrationBuilder.AlterColumn<string>(
                name: "TransactionType",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("06db8d44-0bfc-4af5-b2da-11f62c89b58d"), "ADVISOR" },
                    { new Guid("24def9d6-e203-4cea-97a7-eb5d5c833392"), "LOAN_OFFICER" },
                    { new Guid("506c0808-b82b-4f02-b7ac-80e9c945b506"), "AUDITOR" },
                    { new Guid("8eeea181-0379-457e-b393-035176f0d9af"), "CUSTOMER" },
                    { new Guid("9cbde1d9-61c1-473e-8bab-a0491ea59b1b"), "BANK_OFFICER" },
                    { new Guid("a578a025-7018-4568-a545-7a1fa074af98"), "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("06db8d44-0bfc-4af5-b2da-11f62c89b58d"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("24def9d6-e203-4cea-97a7-eb5d5c833392"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("506c0808-b82b-4f02-b7ac-80e9c945b506"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("8eeea181-0379-457e-b393-035176f0d9af"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("9cbde1d9-61c1-473e-8bab-a0491ea59b1b"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a578a025-7018-4568-a545-7a1fa074af98"));

            migrationBuilder.AlterColumn<int>(
                name: "TransactionType",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("7a50b089-4873-4492-9810-536c23f5357a"), "AUDITOR" },
                    { new Guid("99830320-0fde-4ab5-8dfd-feb5ff4a21ab"), "ADVISOR" },
                    { new Guid("9be1c6d7-7f24-4c3b-bbf7-4cc42a60744c"), "LOAN_OFFICER" },
                    { new Guid("a0100f94-ad41-4128-95f8-168fb679bb93"), "ADMIN" },
                    { new Guid("a60709d5-9612-4314-b47b-0d63fcda119a"), "BANK_OFFICER" },
                    { new Guid("cae19779-86af-490a-9def-bbc17b26371c"), "CUSTOMER" }
                });
        }
    }
}
