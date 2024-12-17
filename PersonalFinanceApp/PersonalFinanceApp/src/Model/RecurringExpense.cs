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
    public int UserID { get; set; }

    //Relationship

    public virtual List<Expense> Expenses { get; set; } = new List<Expense>();

    //public virtual DateTime Date { get; set; } = DateTime.MinValue;

    private RecurringExpense() { }

    public RecurringExpense(string name, string frequency, int interval, DateOnly startDate, int id)
    {
        Name = name;
        Frequency = frequency;
        Interval = interval;
        StartDate = startDate;
        //LastTime = DateOnly.FromDateTime(DateTime.Now);
        LastTime = StartDate;
        UserID = id;
    }
}