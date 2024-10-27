using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceApp.Migrations
{
    /// <inheritdoc />
    public partial class AddNewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Goal",
                table: "USER");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MonthlyIncome",
                table: "USER");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Saving",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "RecurringDate",
                table: "EXPENSE");

            migrationBuilder.RenameColumn(
                name: "MonthlyIncome",
                table: "USER",
                newName: "DefaultBudget");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "EXPENSE",
                newName: "ExpenseID");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "USER",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "USER",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "USER",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "EXPENSESBOOK",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "EXPENSE",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeAdded",
                table: "EXPENSE",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "CATEGORY",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

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

            migrationBuilder.CreateIndex(
                name: "IX_GOAL_UserID",
                table: "GOAL",
                column: "UserID");
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

            migrationBuilder.DropColumn(
                name: "Email",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "USER");

            migrationBuilder.DropColumn(
                name: "TimeAdded",
                table: "EXPENSE");

            migrationBuilder.RenameColumn(
                name: "DefaultBudget",
                table: "USER",
                newName: "MonthlyIncome");

            migrationBuilder.RenameColumn(
                name: "ExpenseID",
                table: "EXPENSE",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "USER",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<long>(
                name: "Goal",
                table: "USER",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "EXPENSESBOOK",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "EXPENSE",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateOnly>(
                name: "RecurringDate",
                table: "EXPENSE",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "CATEGORY",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Goal",
                table: "USER",
                sql: "[Goal] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MonthlyIncome",
                table: "USER",
                sql: "[MonthlyIncome] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Saving",
                table: "USER",
                sql: "[Saving] >= 0");
        }
    }
}
