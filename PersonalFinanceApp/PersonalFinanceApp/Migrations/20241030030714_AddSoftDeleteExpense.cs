using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Amount",
                table: "EXPENSE");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "CATEGORY",
                newName: "CategoryID");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "EXPENSE",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "EXPENSE",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER_Username",
                table: "USER",
                column: "Username",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_DefaultBudget",
                table: "USER",
                sql: "[DefaultBudget] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Saving",
                table: "USER",
                sql: "[Saving] >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_EXPENSE_TimeAdded",
                table: "EXPENSE",
                column: "TimeAdded",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Amount",
                table: "EXPENSE",
                sql: "[Amount] > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Deleted",
                table: "EXPENSE",
                sql: "[Deleted] IN (0, 1)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_USER_Username",
                table: "USER");

            migrationBuilder.DropCheckConstraint(
                name: "CK_DefaultBudget",
                table: "USER");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Saving",
                table: "USER");

            migrationBuilder.DropIndex(
                name: "IX_EXPENSE_TimeAdded",
                table: "EXPENSE");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Amount",
                table: "EXPENSE");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Deleted",
                table: "EXPENSE");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "EXPENSE");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "EXPENSE");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "CATEGORY",
                newName: "ID");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Amount",
                table: "EXPENSE",
                sql: "[Amount] >= 0");
        }
    }
}
