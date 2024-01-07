using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class RolesTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ab640f3-91db-417b-a3dd-024b14837f96");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df167571-d103-4488-b398-52f81c2f2fbd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "efd72960-b554-48b5-88c6-370fca080035");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8ab640f3-91db-417b-a3dd-024b14837f96", "8ab640f3-91db-417b-a3dd-024b14837f96", "Bank_Officer", "BANK_OFFICER" },
                    { "df167571-d103-4488-b398-52f81c2f2fbd", "df167571-d103-4488-b398-52f81c2f2fbd", "Loan_Officer", "LOAN_OFFICER" },
                    { "efd72960-b554-48b5-88c6-370fca080035", "efd72960-b554-48b5-88c6-370fca080035", "Advisor", "ADVISOR" }
                });
        }
    }
}
