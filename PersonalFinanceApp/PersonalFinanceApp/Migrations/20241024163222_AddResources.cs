using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceApp.Migrations
{
    /// <inheritdoc />
    public partial class AddResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Resources",
                table: "USER",
                type: "TEXT",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Resources",
                table: "EXPENSESBOOK",
                type: "TEXT",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Resources",
                table: "EXPENSE",
                type: "TEXT",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Resources",
                table: "CATEGORY",
                type: "TEXT",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resources",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "Resources",
                table: "EXPENSESBOOK");

            migrationBuilder.DropColumn(
                name: "Resources",
                table: "EXPENSE");

            migrationBuilder.DropColumn(
                name: "Resources",
                table: "CATEGORY");
        }
    }
}
