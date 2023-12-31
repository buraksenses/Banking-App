using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class LoanTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfMissedPayments",
                table: "Loans",
                newName: "NumberOfTotalPayments");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTimelyPayments",
                table: "Loans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfTimelyPayments",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "NumberOfTotalPayments",
                table: "Loans",
                newName: "NumberOfMissedPayments");
        }
    }
}
