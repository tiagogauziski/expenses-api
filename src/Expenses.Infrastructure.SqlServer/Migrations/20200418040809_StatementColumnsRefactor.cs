using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.Infrastructure.SqlServer.Migrations
{
    public partial class StatementColumnsRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Statements");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Statements",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Statements",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Statements");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Statements");

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Statements",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
