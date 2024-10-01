using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.Database
{
    public class AppDbContext : DbContext
    {
        DbSet<User> UserTable { get; set; }
        DbSet<ExpensesBook> ExpensesBooksTable { get; set; }
        DbSet<Category> CategoriesTable { get; set; }
        DbSet<Expense> ExpensesTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Setup relationship between table
            //User and ExpensesBooks
            modelBuilder.Entity<User>()
                .HasMany(u => u.ExpensesBooks)
                .WithOne(exB => exB.User)
                .HasForeignKey(exB => exB.UserID);
            //ExpensesBooks and Expense
            modelBuilder.Entity<ExpensesBook>()
                .HasMany(exB => exB.Expenses)
                .WithOne(ex => ex.ExpensesBook)
                .HasForeignKey(ex => ex.ExpensesBookID);
            //ExpensesBooks and Category
            modelBuilder.Entity<ExpensesBook>()
                .HasMany(exB => exB.Categories)
                .WithOne(ca => ca.ExpensesBook)
                .HasForeignKey(ca => ca.ExpensesBookID);
            //Category and expense
            modelBuilder.Entity<Category>()
                .HasMany(ex => ex.Expenses)
                .WithOne(ex => ex.Category)
                .HasForeignKey(ex => ex.CategoryID);
        }
    }
}
