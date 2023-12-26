using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class TransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("100700db-bb7b-44d5-93d1-17b26d9877a7"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("1809731a-a7ba-44cf-b8f7-5fdd224a1a0a"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("266dec10-7b2d-43c8-8108-d47ea391a257"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("3fc2bb3f-42a1-4293-b671-b2a91bcdaa42"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("d55e21e8-86a9-48fd-8030-f414ecc6cbbc"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f1ed393f-d85c-4ae0-b112-21d296b0aa9c"));

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_ReceiverAccountId",
                        column: x => x.ReceiverAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReceiverAccountId",
                table: "Transactions",
                column: "ReceiverAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

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

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("100700db-bb7b-44d5-93d1-17b26d9877a7"), "CUSTOMER" },
                    { new Guid("1809731a-a7ba-44cf-b8f7-5fdd224a1a0a"), "ADMIN" },
                    { new Guid("266dec10-7b2d-43c8-8108-d47ea391a257"), "LOAN_OFFICER" },
                    { new Guid("3fc2bb3f-42a1-4293-b671-b2a91bcdaa42"), "AUDITOR" },
                    { new Guid("d55e21e8-86a9-48fd-8030-f414ecc6cbbc"), "ADVISOR" },
                    { new Guid("f1ed393f-d85c-4ae0-b112-21d296b0aa9c"), "BANK_OFFICER" }
                });
        }
    }
}
