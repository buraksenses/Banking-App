using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class RolesSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
