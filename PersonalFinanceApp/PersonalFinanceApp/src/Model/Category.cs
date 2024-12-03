using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class Category
{
    [Key]
    public int CategoryID { get; set; }

    [Required]
    [MinLength(1)]
    public string Name { get; set; }

    //Relationship
    public virtual List<Expense> Expenses { get; set; } = new List<Expense>();

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

    public Category(string name, int exBMonth, int exBYear, int userId)
    {
        Name = name;
        ExBMonth = exBMonth;
        ExBYear = exBYear;
        UserID = userId;
    }

    public Category(string name, ExpensesBook exB)
    {
        Name = name;
        ExBMonth = exB.Month;
        ExBYear = exB.Year;
        UserID = exB.UserID;
    }
}