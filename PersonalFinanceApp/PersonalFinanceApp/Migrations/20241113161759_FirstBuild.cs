using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceApp.Migrations
{
    /// <inheritdoc />
    public partial class FirstBuild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Saving = table.Column<long>(type: "INTEGER", nullable: false),
                    DefaultBudget = table.Column<long>(type: "INTEGER", nullable: false),
                    Resources = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.UserID);
                    table.CheckConstraint("CK_DefaultBudget", "[DefaultBudget] >= 0");
                    table.CheckConstraint("CK_Saving", "[Saving] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "EXPENSESBOOK",
                columns: table => new
                {
                    Month = table.Column<int>(type: "INTEGER", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false),
                    Budget = table.Column<long>(type: "INTEGER", nullable: false),
                    Resources = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPENSESBOOK", x => new { x.Month, x.Year, x.UserID });
                    table.CheckConstraint("CK_Budget", "[Budget] >= 0");
                    table.CheckConstraint("CK_Month", "[Month] >= 1 AND [Month] <= 12");
                    table.CheckConstraint("CK_Year", "[Year] >= 0");
                    table.ForeignKey(
                        name: "FK_EXPENSESBOOK_USER_UserID",
                        column: x => x.UserID,
                        principalTable: "USER",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GOAL",
                columns: table => new
                {
                    GoalID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Target = table.Column<long>(type: "INTEGER", nullable: false),
                    GoalAmount = table.Column<long>(type: "INTEGER", nullable: false),
                    Resources = table.Column<string>(type: "TEXT", nullable: true),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GOAL", x => x.GoalID);
                    table.ForeignKey(
                        name: "FK_GOAL_USER_UserID",
                        column: x => x.UserID,
                        principalTable: "USER",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CATEGORY",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Resources = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ExBMonth = table.Column<int>(type: "INTEGER", nullable: false),
                    ExBYear = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORY", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_CATEGORY_EXPENSESBOOK_ExBMonth_ExBYear_UserID",
                        columns: x => new { x.ExBMonth, x.ExBYear, x.UserID },
                        principalTable: "EXPENSESBOOK",
                        principalColumns: new[] { "Month", "Year", "UserID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GOALHISTORY",
                columns: table => new
                {
                    GoalID = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeAdded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GOALHISTORY", x => new { x.GoalID, x.TimeAdded });
                    table.ForeignKey(
                        name: "FK_GOALHISTORY_GOAL_GoalID",
                        column: x => x.GoalID,
                        principalTable: "GOAL",
                        principalColumn: "GoalID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EXPENSE",
                columns: table => new
                {
                    ExpenseID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Recurring = table.Column<bool>(type: "INTEGER", nullable: false),
                    Resources = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    TimeAdded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CategoryID = table.Column<int>(type: "INTEGER", nullable: false),
                    ExBMonth = table.Column<int>(type: "INTEGER", nullable: false),
                    ExBYear = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EXPENSE", x => x.ExpenseID);
                    table.CheckConstraint("CK_Amount", "[Amount] > 0");
                    table.CheckConstraint("CK_Deleted", "[Deleted] IN (0, 1)");
                    table.CheckConstraint("CK_Recurring", "[Recurring] IN (0, 1)");
                    table.ForeignKey(
                        name: "FK_EXPENSE_CATEGORY_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "CATEGORY",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EXPENSE_EXPENSESBOOK_ExBMonth_ExBYear_UserID",
                        columns: x => new { x.ExBMonth, x.ExBYear, x.UserID },
                        principalTable: "EXPENSESBOOK",
                        principalColumns: new[] { "Month", "Year", "UserID" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RECURRINGDETAIL",
                columns: table => new
                {
                    ExpenseID = table.Column<int>(type: "INTEGER", nullable: false),
                    Frequency = table.Column<string>(type: "TEXT", nullable: false),
                    Interval = table.Column<int>(type: "INTEGER", nullable: false),
                    StarDate = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECURRINGDETAIL", x => x.ExpenseID);
                    table.CheckConstraint("CK_Frequency", "[Frequency] IN ('Daily', 'Weekly', 'Monthly', 'Yearly')");
                    table.ForeignKey(
                        name: "FK_RECURRINGDETAIL_EXPENSE_ExpenseID",
                        column: x => x.ExpenseID,
                        principalTable: "EXPENSE",
                        principalColumn: "ExpenseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CATEGORY_ExBMonth_ExBYear_UserID",
                table: "CATEGORY",
                columns: new[] { "ExBMonth", "ExBYear", "UserID" });

            migrationBuilder.CreateIndex(
                name: "IX_EXPENSE_CategoryID",
                table: "EXPENSE",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_EXPENSE_Date",
                table: "EXPENSE",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_EXPENSE_ExBMonth_ExBYear_UserID",
                table: "EXPENSE",
                columns: new[] { "ExBMonth", "ExBYear", "UserID" });

            migrationBuilder.CreateIndex(
                name: "IX_EXPENSE_TimeAdded",
                table: "EXPENSE",
                column: "TimeAdded",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EXPENSESBOOK_UserID",
                table: "EXPENSESBOOK",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_GOAL_UserID",
                table: "GOAL",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_USER_Username",
                table: "USER",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GOALHISTORY");

            migrationBuilder.DropTable(
                name: "RECURRINGDETAIL");

            migrationBuilder.DropTable(
                name: "GOAL");

            migrationBuilder.DropTable(
                name: "EXPENSE");

            migrationBuilder.DropTable(
                name: "CATEGORY");

            migrationBuilder.DropTable(
                name: "EXPENSESBOOK");

            migrationBuilder.DropTable(
                name: "USER");
        }
    }
}
