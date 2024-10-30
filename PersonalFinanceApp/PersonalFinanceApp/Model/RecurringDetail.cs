using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class RecurringDetail
{
    [Key]
    public int ExpenseID { get; set; }

    [Required]
    public string Frequency { get; set; }

    [Required]
    public int Interval { get; set; }

    [Required]
    public DateOnly StarDate { get; set; }

    //Relationship

    public virtual Expense? Expense { get; set; }

    public RecurringDetail() { }

    public RecurringDetail(string frequency, int interval, DateOnly starDate)
    {
        Frequency = frequency;
        Interval = interval;
        StarDate = starDate;
    }
}