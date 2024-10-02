using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTable",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Saving = table.Column<long>(type: "INTEGER", nullable: false),
                    Goal = table.Column<long>(type: "INTEGER", nullable: false),
                    MonthlyIncome = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTable", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "ExpensesBooksTable",
                columns: table => new
                {
                    Month = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Budget = table.Column<long>(type: "INTEGER", nullable: false),
                    Spending = table.Column<long>(type: "INTEGER", nullable: false),
                    UserID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensesBooksTable", x => new { x.Month, x.Year });
                    table.ForeignKey(
                        name: "FK_ExpensesBooksTable_UserTable_UserID",
                        column: x => x.UserID,
                        principalTable: "UserTable",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriesTable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ExBMonth = table.Column<int>(type: "INTEGER", nullable: false),
                    ExBYear = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesTable", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CategoriesTable_ExpensesBooksTable_ExBMonth_ExBYear",
                        columns: x => new { x.ExBMonth, x.ExBYear },
                        principalTable: "ExpensesBooksTable",
                        principalColumns: new[] { "Month", "Year" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpensesTable",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Recurring = table.Column<bool>(type: "INTEGER", nullable: false),
                    RecurringDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    CategoryID = table.Column<int>(type: "INTEGER", nullable: false),
                    ExBMonth = table.Column<int>(type: "INTEGER", nullable: false),
                    ExBYear = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensesTable", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExpensesTable_CategoriesTable_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "CategoriesTable",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpensesTable_ExpensesBooksTable_ExBMonth_ExBYear",
                        columns: x => new { x.ExBMonth, x.ExBYear },
                        principalTable: "ExpensesBooksTable",
                        principalColumns: new[] { "Month", "Year" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesTable_ExBMonth_ExBYear",
                table: "CategoriesTable",
                columns: new[] { "ExBMonth", "ExBYear" });

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesBooksTable_UserID",
                table: "ExpensesBooksTable",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesTable_CategoryID",
                table: "ExpensesTable",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesTable_ExBMonth_ExBYear",
                table: "ExpensesTable",
                columns: new[] { "ExBMonth", "ExBYear" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpensesTable");

            migrationBuilder.DropTable(
                name: "CategoriesTable");

            migrationBuilder.DropTable(
                name: "ExpensesBooksTable");

            migrationBuilder.DropTable(
                name: "UserTable");
        }
    }
}
