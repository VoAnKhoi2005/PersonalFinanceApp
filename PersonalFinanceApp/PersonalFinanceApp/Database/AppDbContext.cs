using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.Database
{
    public class AppDbContext : DbContext
    {
        //Table
        public DbSet<User> USER { get; set; }
        public DbSet<ExpensesBook> EXPENSESBOOK { get; set; }
        public DbSet<Category> CATEGORY { get; set; }
        public DbSet<Expense> EXPENSE { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=PFA.db");
        }

        public void EnsureDatabaseCreated()
        {
            Database.EnsureCreated();
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

            //Setup composite key for expense book
            modelBuilder.Entity<ExpensesBook>().HasKey(exB => new { exB.Month, exB.Year, exB.UserID });
            //Setup relationship between table
            //ExpensesBooks and Expense
            modelBuilder.Entity<ExpensesBook>()
                .HasMany(exB => exB.Expenses)
                .WithOne(ex => ex.ExpensesBook)
                .HasForeignKey(ex => new {ex.ExBMonth, ex.ExBYear, ex.UserID});
            //User and ExpensesBooks
            modelBuilder.Entity<User>()
                .HasMany(u => u.ExpensesBooks)
                .WithOne(exB => exB.User)
                .HasForeignKey(exB => exB.UserID);
            //ExpensesBooks and Category
            modelBuilder.Entity<ExpensesBook>()
                .HasMany(exB => exB.Categories)
                .WithOne(ca => ca.ExpensesBook)
                .HasForeignKey(ca => new {ca.ExBMonth, ca.ExBYear, ca.UserID});
            //Category and expense
            modelBuilder.Entity<Category>()
                .HasMany(ex => ex.Expenses)
                .WithOne(ex => ex.Category)
                .HasForeignKey(ex => ex.CategoryID);

            //Setup index
            modelBuilder.Entity<Expense>().HasIndex(ex => ex.Date);
        }
    }
}
