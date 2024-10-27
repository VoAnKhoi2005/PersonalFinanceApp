using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceApp.Migrations
{
    /// <inheritdoc />
    public partial class RenameDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriesTable_ExpensesBooksTable_ExBMonth_ExBYear_UserID",
                table: "CategoriesTable");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensesBooksTable_UserTable_UserID",
                table: "ExpensesBooksTable");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensesTable_CategoriesTable_CategoryID",
                table: "ExpensesTable");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpensesTable_ExpensesBooksTable_ExBMonth_ExBYear_UserID",
                table: "ExpensesTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTable",
                table: "UserTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpensesTable",
                table: "ExpensesTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpensesBooksTable",
                table: "ExpensesBooksTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoriesTable",
                table: "CategoriesTable");

            migrationBuilder.RenameTable(
                name: "UserTable",
                newName: "USER");

            migrationBuilder.RenameTable(
                name: "ExpensesTable",
                newName: "EXPENSE");

            migrationBuilder.RenameTable(
                name: "ExpensesBooksTable",
                newName: "EXPENSESBOOK");

            migrationBuilder.RenameTable(
                name: "CategoriesTable",
                newName: "CATEGORY");

            migrationBuilder.RenameIndex(
                name: "IX_ExpensesTable_ExBMonth_ExBYear_UserID",
                table: "EXPENSE",
                newName: "IX_EXPENSE_ExBMonth_ExBYear_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_ExpensesTable_Date",
                table: "EXPENSE",
                newName: "IX_EXPENSE_Date");

            migrationBuilder.RenameIndex(
                name: "IX_ExpensesTable_CategoryID",
                table: "EXPENSE",
                newName: "IX_EXPENSE_CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_ExpensesBooksTable_UserID",
                table: "EXPENSESBOOK",
                newName: "IX_EXPENSESBOOK_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_CategoriesTable_ExBMonth_ExBYear_UserID",
                table: "CATEGORY",
                newName: "IX_CATEGORY_ExBMonth_ExBYear_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_USER",
                table: "USER",
                column: "UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EXPENSE",
                table: "EXPENSE",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EXPENSESBOOK",
                table: "EXPENSESBOOK",
                columns: new[] { "Month", "Year", "UserID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CATEGORY",
                table: "CATEGORY",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CATEGORY_EXPENSESBOOK_ExBMonth_ExBYear_UserID",
                table: "CATEGORY",
                columns: new[] { "ExBMonth", "ExBYear", "UserID" },
                principalTable: "EXPENSESBOOK",
                principalColumns: new[] { "Month", "Year", "UserID" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EXPENSE_CATEGORY_CategoryID",
                table: "EXPENSE",
                column: "CategoryID",
                principalTable: "CATEGORY",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EXPENSE_EXPENSESBOOK_ExBMonth_ExBYear_UserID",
                table: "EXPENSE",
                columns: new[] { "ExBMonth", "ExBYear", "UserID" },
                principalTable: "EXPENSESBOOK",
                principalColumns: new[] { "Month", "Year", "UserID" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EXPENSESBOOK_USER_UserID",
                table: "EXPENSESBOOK",
                column: "UserID",
                principalTable: "USER",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CATEGORY_EXPENSESBOOK_ExBMonth_ExBYear_UserID",
                table: "CATEGORY");

            migrationBuilder.DropForeignKey(
                name: "FK_EXPENSE_CATEGORY_CategoryID",
                table: "EXPENSE");

            migrationBuilder.DropForeignKey(
                name: "FK_EXPENSE_EXPENSESBOOK_ExBMonth_ExBYear_UserID",
                table: "EXPENSE");

            migrationBuilder.DropForeignKey(
                name: "FK_EXPENSESBOOK_USER_UserID",
                table: "EXPENSESBOOK");

            migrationBuilder.DropPrimaryKey(
                name: "PK_USER",
                table: "USER");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EXPENSESBOOK",
                table: "EXPENSESBOOK");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EXPENSE",
                table: "EXPENSE");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CATEGORY",
                table: "CATEGORY");

            migrationBuilder.RenameTable(
                name: "USER",
                newName: "UserTable");

            migrationBuilder.RenameTable(
                name: "EXPENSESBOOK",
                newName: "ExpensesBooksTable");

            migrationBuilder.RenameTable(
                name: "EXPENSE",
                newName: "ExpensesTable");

            migrationBuilder.RenameTable(
                name: "CATEGORY",
                newName: "CategoriesTable");

            migrationBuilder.RenameIndex(
                name: "IX_EXPENSESBOOK_UserID",
                table: "ExpensesBooksTable",
                newName: "IX_ExpensesBooksTable_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_EXPENSE_ExBMonth_ExBYear_UserID",
                table: "ExpensesTable",
                newName: "IX_ExpensesTable_ExBMonth_ExBYear_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_EXPENSE_Date",
                table: "ExpensesTable",
                newName: "IX_ExpensesTable_Date");

            migrationBuilder.RenameIndex(
                name: "IX_EXPENSE_CategoryID",
                table: "ExpensesTable",
                newName: "IX_ExpensesTable_CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_CATEGORY_ExBMonth_ExBYear_UserID",
                table: "CategoriesTable",
                newName: "IX_CategoriesTable_ExBMonth_ExBYear_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTable",
                table: "UserTable",
                column: "UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpensesBooksTable",
                table: "ExpensesBooksTable",
                columns: new[] { "Month", "Year", "UserID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpensesTable",
                table: "ExpensesTable",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoriesTable",
                table: "CategoriesTable",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriesTable_ExpensesBooksTable_ExBMonth_ExBYear_UserID",
                table: "CategoriesTable",
                columns: new[] { "ExBMonth", "ExBYear", "UserID" },
                principalTable: "ExpensesBooksTable",
                principalColumns: new[] { "Month", "Year", "UserID" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensesBooksTable_UserTable_UserID",
                table: "ExpensesBooksTable",
                column: "UserID",
                principalTable: "UserTable",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensesTable_CategoriesTable_CategoryID",
                table: "ExpensesTable",
                column: "CategoryID",
                principalTable: "CategoriesTable",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpensesTable_ExpensesBooksTable_ExBMonth_ExBYear_UserID",
                table: "ExpensesTable",
                columns: new[] { "ExBMonth", "ExBYear", "UserID" },
                principalTable: "ExpensesBooksTable",
                principalColumns: new[] { "Month", "Year", "UserID" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
