using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreditHistoryDetailsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasLatePayments",
                table: "CreditHistoryDetails");

            migrationBuilder.DropColumn(
                name: "CreditScore",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<float>(
                name: "LoanAmount",
                table: "CreditHistoryDetails",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "LoanTerm",
                table: "CreditHistoryDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanTerm",
                table: "CreditHistoryDetails");

            migrationBuilder.AlterColumn<decimal>(
                name: "LoanAmount",
                table: "CreditHistoryDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<bool>(
                name: "HasLatePayments",
                table: "CreditHistoryDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "CreditScore",
                table: "AspNetUsers",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
