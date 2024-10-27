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
    public Expense Expense { get; set; }

}