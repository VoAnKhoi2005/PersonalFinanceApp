using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class User
{
    [Key]
    public string UserID { get; set; }

    [Required]
    [MaxLength(40)]
    public string Username { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }

    [Range(0, 1000000000000000)]
    public long Saving { get; set; }

    [Range(0, 1000000000000000)]
    public long Goal { get; set; }

    [Range(0, 1000000000000000)]
    public long MonthlyIncome { get; set; }

    [MaxLength(256)]
    public string? Resources { get; set; }

    //Relationship
    public virtual List<ExpensesBook> ExpensesBooks { get; set; } = new List<ExpensesBook>();
}