using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sauvio.Migrations
{
    /// <inheritdoc />
    public partial class AddFinanceFieldsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "users",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalExpense",
                table: "users",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalIncome",
                table: "users",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "users");

            migrationBuilder.DropColumn(
                name: "TotalExpense",
                table: "users");

            migrationBuilder.DropColumn(
                name: "TotalIncome",
                table: "users");
        }
    }
}
