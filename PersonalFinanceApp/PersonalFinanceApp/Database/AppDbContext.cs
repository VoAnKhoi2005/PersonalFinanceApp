using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.Database
{
    public class AppDbContext : DbContext
    {
        //Table
        public DbSet<User> UserTable { get; set; }
        public DbSet<ExpensesBook> ExpensesBooksTable { get; set; }
        public DbSet<Category> CategoriesTable { get; set; }
        public DbSet<Expense> ExpensesTable { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=PFA.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Setup constraint
            //User
            modelBuilder.Entity<User>().ToTable(u => u.HasCheckConstraint("CK_Saving", "[Saving] >= 0"));
            modelBuilder.Entity<User>().ToTable(u => u.HasCheckConstraint("CK_Goal", "[Goal] >= 0"));
            modelBuilder.Entity<User>().ToTable(u => u.HasCheckConstraint("CK_MonthlyIncome", "[MonthlyIncome] >= 0"));
            //Expense
            modelBuilder.Entity<Expense>().ToTable(ex => ex.HasCheckConstraint("CK_Amount", "[Amount] >= 0"));
            modelBuilder.Entity<Expense>().ToTable(ex => ex.HasCheckConstraint("CK_Recurring", "[Recurring] IN (0, 1)"));
            modelBuilder.Entity<Expense>().Property(ex => ex.Description).IsRequired(false);
            modelBuilder.Entity<Expense>().Property(ex => ex.RecurringDate).IsRequired(false);
            //Expenses Book
            modelBuilder.Entity<ExpensesBook>().ToTable(exB => exB.HasCheckConstraint("CK_Month", "[Month] >= 1 AND [Month] <= 12"));  
            modelBuilder.Entity<ExpensesBook>().ToTable(exB => exB.HasCheckConstraint("CK_Year", "[Year] >= 0"));
            modelBuilder.Entity<ExpensesBook>().ToTable(exB => exB.HasCheckConstraint("CK_Budget", "[Budget] >= 0"));
            modelBuilder.Entity<ExpensesBook>().ToTable(exB => exB.HasCheckConstraint("CK_Spending", "[Spending] >= 0"));

            //Setup relationship between table
            //Setup composite key for expense book
            modelBuilder.Entity<ExpensesBook>().HasKey(exB => new { exB.Month, exB.Year });
            //ExpensesBooks and Expense
            modelBuilder.Entity<ExpensesBook>()
                .HasMany(exB => exB.Expenses)
                .WithOne(ex => ex.ExpensesBook)
                .HasForeignKey(ex => new {ex.ExBMonth, ex.ExBYear});
            //User and ExpensesBooks
            modelBuilder.Entity<User>()
                .HasMany(u => u.ExpensesBooks)
                .WithOne(exB => exB.User)
                .HasForeignKey(exB => exB.UserID);
            //ExpensesBooks and Category
            modelBuilder.Entity<ExpensesBook>()
                .HasMany(exB => exB.Categories)
                .WithOne(ca => ca.ExpensesBook)
                .HasForeignKey(ca => new {ca.ExBMonth, ca.ExBYear});
            //Category and expense
            modelBuilder.Entity<Category>()
                .HasMany(ex => ex.Expenses)
                .WithOne(ex => ex.Category)
                .HasForeignKey(ex => ex.CategoryID);
        }
    }
}
