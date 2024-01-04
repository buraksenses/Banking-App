using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class TransactionApplicationsTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionApplications_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50df0d64-b2ae-482a-98d1-bd187fbbbeda",
                column: "NormalizedName",
                value: "ADMIN");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d6eb8c4-151c-45ec-9083-9d8877852e78",
                column: "NormalizedName",
                value: "AUDITOR");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ab640f3-91db-417b-a3dd-024b14837f96",
                column: "NormalizedName",
                value: "BANK_OFFICER");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df167571-d103-4488-b398-52f81c2f2fbd",
                column: "NormalizedName",
                value: "LOAN_OFFICER");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "efd72960-b554-48b5-88c6-370fca080035",
                column: "NormalizedName",
                value: "ADVISOR");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionApplications_AccountId",
                table: "TransactionApplications",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionApplications");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "50df0d64-b2ae-482a-98d1-bd187fbbbeda",
                column: "NormalizedName",
                value: "ADMİN");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d6eb8c4-151c-45ec-9083-9d8877852e78",
                column: "NormalizedName",
                value: "AUDİTOR");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ab640f3-91db-417b-a3dd-024b14837f96",
                column: "NormalizedName",
                value: "BANK_OFFİCER");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df167571-d103-4488-b398-52f81c2f2fbd",
                column: "NormalizedName",
                value: "LOAN_OFFİCER");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "efd72960-b554-48b5-88c6-370fca080035",
                column: "NormalizedName",
                value: "ADVİSOR");
        }
    }
}
