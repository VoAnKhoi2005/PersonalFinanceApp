using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceApp.Migrations
{
    /// <inheritdoc />
    public partial class Create : Migration
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
                    table.CheckConstraint("CK_Goal", "[Goal] >= 0");
                    table.CheckConstraint("CK_MonthlyIncome", "[MonthlyIncome] >= 0");
                    table.CheckConstraint("CK_Saving", "[Saving] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "ExpensesBooksTable",
                columns: table => new
                {
                    Month = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<string>(type: "TEXT", nullable: false),
                    Budget = table.Column<long>(type: "INTEGER", nullable: false),
                    Spending = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensesBooksTable", x => new { x.Month, x.Year, x.UserID });
                    table.CheckConstraint("CK_Budget", "[Budget] >= 0");
                    table.CheckConstraint("CK_Month", "[Month] >= 1 AND [Month] <= 12");
                    table.CheckConstraint("CK_Spending", "[Spending] >= 0");
                    table.CheckConstraint("CK_Year", "[Year] >= 0");
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
                    ExBYear = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriesTable", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CategoriesTable_ExpensesBooksTable_ExBMonth_ExBYear_UserID",
                        columns: x => new { x.ExBMonth, x.ExBYear, x.UserID },
                        principalTable: "ExpensesBooksTable",
                        principalColumns: new[] { "Month", "Year", "UserID" },
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
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Recurring = table.Column<bool>(type: "INTEGER", nullable: false),
                    RecurringDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    CategoryID = table.Column<int>(type: "INTEGER", nullable: false),
                    ExBMonth = table.Column<int>(type: "INTEGER", nullable: false),
                    ExBYear = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensesTable", x => x.ID);
                    table.CheckConstraint("CK_Amount", "[Amount] >= 0");
                    table.CheckConstraint("CK_Recurring", "[Recurring] IN (0, 1)");
                    table.ForeignKey(
                        name: "FK_ExpensesTable_CategoriesTable_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "CategoriesTable",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpensesTable_ExpensesBooksTable_ExBMonth_ExBYear_UserID",
                        columns: x => new { x.ExBMonth, x.ExBYear, x.UserID },
                        principalTable: "ExpensesBooksTable",
                        principalColumns: new[] { "Month", "Year", "UserID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriesTable_ExBMonth_ExBYear_UserID",
                table: "CategoriesTable",
                columns: new[] { "ExBMonth", "ExBYear", "UserID" });

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesBooksTable_UserID",
                table: "ExpensesBooksTable",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesTable_CategoryID",
                table: "ExpensesTable",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesTable_Date",
                table: "ExpensesTable",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesTable_ExBMonth_ExBYear_UserID",
                table: "ExpensesTable",
                columns: new[] { "ExBMonth", "ExBYear", "UserID" });
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
