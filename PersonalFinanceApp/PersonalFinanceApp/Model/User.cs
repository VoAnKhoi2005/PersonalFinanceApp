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

    [Range(0, 100000000000000)]
    public decimal Saving { get; set; }

    [Range(0, 100000000000000)]
    public decimal Goal { get; set; }

    [Range(0, 100000000000000)]
    public decimal MonthlyIncome { get; set; }

    public virtual List<ExpensesBook> ExpensesBooks { get; set; } = new List<ExpensesBook>();
}