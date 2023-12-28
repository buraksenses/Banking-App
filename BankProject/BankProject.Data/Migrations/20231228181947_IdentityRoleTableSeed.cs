using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class IdentityRoleTableSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "50df0d64-b2ae-482a-98d1-bd187fbbbeda", "50df0d64-b2ae-482a-98d1-bd187fbbbeda", "Admin", "ADMİN" },
                    { "6d6eb8c4-151c-45ec-9083-9d8877852e78", "6d6eb8c4-151c-45ec-9083-9d8877852e78", "Auditor", "AUDİTOR" },
                    { "8ab640f3-91db-417b-a3dd-024b14837f96", "8ab640f3-91db-417b-a3dd-024b14837f96", "Bank_Officer", "BANK_OFFİCER" },
                    { "ab24b60e-d36f-4662-ab2c-0faefdb86d3e", "ab24b60e-d36f-4662-ab2c-0faefdb86d3e", "Customer", "CUSTOMER" },
                    { "df167571-d103-4488-b398-52f81c2f2fbd", "df167571-d103-4488-b398-52f81c2f2fbd", "Loan_Officer", "LOAN_OFFİCER" },
                    { "efd72960-b554-48b5-88c6-370fca080035", "efd72960-b554-48b5-88c6-370fca080035", "Advisor", "ADVİSOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50df0d64-b2ae-482a-98d1-bd187fbbbeda");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d6eb8c4-151c-45ec-9083-9d8877852e78");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ab640f3-91db-417b-a3dd-024b14837f96");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab24b60e-d36f-4662-ab2c-0faefdb86d3e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df167571-d103-4488-b398-52f81c2f2fbd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "efd72960-b554-48b5-88c6-370fca080035");
        }
    }
}
