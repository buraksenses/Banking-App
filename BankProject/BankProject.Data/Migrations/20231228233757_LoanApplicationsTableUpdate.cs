using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class LoanApplicationsTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoanApplicationStatus",
                table: "LoanApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanApplicationStatus",
                table: "LoanApplications");
        }
    }
}
