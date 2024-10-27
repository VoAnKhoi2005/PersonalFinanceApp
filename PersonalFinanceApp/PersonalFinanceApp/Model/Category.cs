using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class Category
{
    [Key]
    public int CategoryID { get; set; }

    [Required]
    [MinLength(1)]
    public string Name { get; set; }

    [MaxLength(256)]
    public string? Resources { get; set; }

    //Relationship
    public virtual List<Expense>? Expenses { get; set; }

    [Required]
    [Range(1,12)]
    public int ExBMonth { get; set; }
    [Required]
    [Range(1,3000)]
    public int ExBYear { get; set; }
    [Required]
    public int UserID { get; set; }

    public virtual ExpensesBook? ExpensesBook { get; set; }

    public Category() { }

    public Category(string name, int exBMonth, int exBYear, int userId, string? resources = null)
    {
        Name = name;
        ExBMonth = exBMonth;
        ExBYear = exBYear;
        UserID = userId;
        Resources = resources;
    }

    public Category(string name, ExpensesBook exB, string? resources = null)
    {
        Name = name;
        ExBMonth = exB.Month;
        ExBYear = exB.Year;
        UserID = exB.UserID;
        Resources = resources;
    }
}