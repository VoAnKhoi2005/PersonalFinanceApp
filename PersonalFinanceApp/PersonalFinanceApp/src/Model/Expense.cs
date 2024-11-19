using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class Expense
{
    [Key]
    public int ExpenseID { get; set; }

    [Required]
    [Range(1,1000000000000)]
    public long Amount { get; set; }

    [Required]
    [MinLength(1)]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required]
    public DateOnly Date { get; set; }

    [Required]
    public bool Recurring { get; set; }

    [MaxLength(256)]
    public string? Resources { get; set; }

    [Required]
    public DateTime TimeAdded { get; set; }

    [Required]
    public bool Deleted { get; set; }
    public DateTime? DeletedDate { get; set; }

    //Relationship
    [Required]
    public int CategoryID { get; set; }
    public virtual Category? Category { get; set; }

    [Required]
    [Range(1,12)]
    public int ExBMonth { get; set; }
    [Required]
    [Range(1,3000)]
    public int ExBYear { get; set; }
    [Required]
    public int UserID { get; set; }
    public virtual ExpensesBook? ExpensesBook { get; set; }

    public virtual RecurringDetail? RecurringDetail { get; set; }

    private Expense() { }

    public Expense(long amount, string name, DateOnly date, bool recurring, int categoryId, int exBMonth, int exBYear, int userId, string? description = null, string? resources = null)
    {
        Amount = amount;
        Name = name;
        Date = date;
        Recurring = recurring;
        CategoryID = categoryId;
        ExBMonth = exBMonth;
        ExBYear = exBYear;
        UserID = userId;
        Description = description;
        TimeAdded = DateTime.Now;
        Resources = resources;
        Deleted = false;
    }

    public Expense(long amount, string name, DateOnly date, bool recurring, Category ca, ExpensesBook exB, string? description = null, string? resources = null)
    {
        Amount = amount;
        Name = name;
        Date = date;
        Recurring = recurring;
        CategoryID = ca.CategoryID;
        ExBMonth = exB.Month;
        ExBYear = exB.Year;
        UserID = exB.UserID;
        Description = description;
        TimeAdded = DateTime.Now;
        Resources = resources;
        Deleted = false;
    }
}