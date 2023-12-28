using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreditHistoryDetailsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditHistoryDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoanDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoanAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HasLatePayments = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfLatePayments = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditHistoryDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditHistoryDetails_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditHistoryDetails_UserId",
                table: "CreditHistoryDetails",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditHistoryDetails");
        }
    }
}
