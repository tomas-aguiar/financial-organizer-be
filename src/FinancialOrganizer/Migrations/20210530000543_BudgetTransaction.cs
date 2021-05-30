using Microsoft.EntityFrameworkCore.Migrations;

namespace FinancialOrganizer.Migrations
{
    public partial class BudgetTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBudget",
                table: "Transactions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBudget",
                table: "Transactions");
        }
    }
}
