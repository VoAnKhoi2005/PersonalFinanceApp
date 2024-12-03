using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class RecurringExpense
{
    [Key]
    public int RecurringExpenseID { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Frequency { get; set; }

    [Required]
    public int Interval { get; set; }

    [Required]
    public DateOnly StarDate { get; set; }

    //Relationship

    public virtual List<Expense> Expenses { get; set; } = new List<Expense>();

    private RecurringExpense() { }

    public RecurringExpense(string name, string frequency, int interval, DateOnly starDate)
    {
        Name = name;
        Frequency = frequency;
        Interval = interval;
        StarDate = starDate;
    }
}