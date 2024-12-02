using System.Configuration;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Model;
using SQLitePCL;

namespace PersonalFinanceApp.Database;

public class AppDbContext : DbContext
{
    //Table
    public DbSet<User> USER { get; set; }
    public DbSet<ExpensesBook> EXPENSESBOOK { get; set; }
    public DbSet<Category> CATEGORY { get; set; }
    public DbSet<Expense> EXPENSE { get; set; }
    public DbSet<RecurringDetail> RECURRINGDETAIL { get; set; }
    public DbSet<Goal> GOAL { get; set; }
    public DbSet<GoalCategory> GOALCATEGORY { get; set; }
    public DbSet<GoalHistory> GOALHISTORY { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connStr = ConfigurationManager.ConnectionStrings["MyDatabase"]?.ConnectionString;
        if (connStr == null)
            throw new InvalidOperationException("Connection string not found.");
        optionsBuilder.UseSqlite(connStr);
        //optionsBuilder.UseSqlite("Data Source=PFA.db");
    }

    public void EnsureDatabaseCreated()
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Setup constraint
        //User
        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<User>().ToTable(u => u.HasCheckConstraint("CK_DefaultBudget", "[DefaultBudget] >= 0"));
        modelBuilder.Entity<User>().ToTable(u => u.HasCheckConstraint("CK_Saving", "[Saving] >= 0"));
        //Expense
        modelBuilder.Entity<Expense>().HasIndex(ex => ex.Date);
        modelBuilder.Entity<Expense>().HasIndex(ex => ex.TimeAdded).IsUnique();
        modelBuilder.Entity<Expense>().Property(ex => ex.Description).IsRequired(false);
        modelBuilder.Entity<Expense>().Property(ex => ex.Resources).IsRequired(false);
        modelBuilder.Entity<Expense>().Property(ex => ex.DeletedDate).IsRequired(false);
        modelBuilder.Entity<Expense>().ToTable(ex => ex.HasCheckConstraint("CK_Amount", "[Amount] > 0"));
        //Expenses Book
        modelBuilder.Entity<ExpensesBook>().ToTable(exB => exB.HasCheckConstraint("CK_Month", "[Month] >= 1 AND [Month] <= 12"));
        modelBuilder.Entity<ExpensesBook>().ToTable(exB => exB.HasCheckConstraint("CK_Year", "[Year] >= 0"));
        modelBuilder.Entity<ExpensesBook>().ToTable(exB => exB.HasCheckConstraint("CK_Budget", "[Budget] >= 0"));
        //RecurringExpense
        modelBuilder.Entity<RecurringDetail>().ToTable(rd => rd.HasCheckConstraint("CK_Frequency", "[Frequency] IN ('Daily', 'Weekly', 'Monthly', 'Yearly')"));
        //Goal
        modelBuilder.Entity<Goal>().Property(g => g.Deadline).IsRequired(false);
        modelBuilder.Entity<Goal>().ToTable(g => g.HasCheckConstraint("CK_Reminder", "[Reminder] IN ('Daily', 'Weekly', 'Monthly', 'Yearly')"));
        modelBuilder.Entity<Goal>().ToTable(g => g.HasCheckConstraint("CK_Status", "[Status] IN ('Completed', 'Active', 'Canceled')"));

        //Setup composite key for ExpensesBook and GoalHistory
        modelBuilder.Entity<ExpensesBook>().HasKey(exB => new { exB.Month, exB.Year, exB.UserID });
        modelBuilder.Entity<GoalHistory>().HasKey(gh => new { gh.GoalID, gh.TimeAdded });

        //Setup relationship between table
        //ExpensesBooks and Expense
        modelBuilder.Entity<ExpensesBook>()
            .HasMany(exB => exB.Expenses)
            .WithOne(ex => ex.ExpensesBook)
            .HasForeignKey(ex => new { ex.ExBMonth, ex.ExBYear, ex.UserID });
        //User and ExpensesBooks
        modelBuilder.Entity<User>()
            .HasMany(u => u.ExpensesBooks)
            .WithOne(exB => exB.User)
            .HasForeignKey(exB => exB.UserID);
        //ExpensesBooks and Category
        modelBuilder.Entity<ExpensesBook>()
            .HasMany(exB => exB.Categories)
            .WithOne(ca => ca.ExpensesBook)
            .HasForeignKey(ca => new { ca.ExBMonth, ca.ExBYear, ca.UserID });
        //Category and expense
        modelBuilder.Entity<Category>()
            .HasMany(ca => ca.Expenses)
            .WithOne(ex => ex.Category)
            .HasForeignKey(ex => ex.CategoryID);
        //User and Goal
        modelBuilder.Entity<User>()
            .HasMany(u => u.Goals)
            .WithOne(g => g.User)
            .HasForeignKey(g => g.UserID);
        //Goal and GoalHistory
        modelBuilder.Entity<Goal>()
            .HasMany(g => g.GoalHistories)
            .WithOne(gh => gh.Goal)
            .HasForeignKey(gh => gh.GoalID);
        //Goal and GoalCategory
        modelBuilder.Entity<GoalCategory>()
            .HasMany(gc => gc.Goals)
            .WithOne(g => g.GoalCategory)
            .HasForeignKey(g => g.CategoryName);
        //Expense and RecurringDetail
        modelBuilder.Entity<Expense>()
            .HasOne(ex => ex.RecurringDetail)
            .WithOne(rd => rd.Expense)
            .HasForeignKey<RecurringDetail>(rd => rd.ExpenseID);
    }
}