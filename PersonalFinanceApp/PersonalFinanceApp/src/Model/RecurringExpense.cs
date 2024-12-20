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
    public DateOnly StartDate { get; set; }

    [Required]
    public DateOnly LastTime { get; set; }

    [Required]
    public string Status { get; set; }

    [Required] 
    public int UserID { get; set; }

    //Relationship

    public virtual List<Expense> Expenses { get; set; } = new List<Expense>();

    private RecurringExpense() { }

    public RecurringExpense(string name, string frequency, int interval, DateOnly startDate, int id)
    {
        Name = name;
        Frequency = frequency;
        Interval = interval;
        StartDate = startDate;
        LastTime = StartDate;
        UserID = id;
        Status = "Active";
    }
}