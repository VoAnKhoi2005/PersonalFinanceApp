using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model;

public class Category
{
    [Key]
    public int ID { get; set; }

    [Required]
    [MinLength(1)]
    public string Name { get; set; }

    [MaxLength(256)]
    public string? Resources { get; set; }

    //Relationship
    public virtual List<Expense> Expenses { get; set; }

    [Required]
    [Range(1,12)]
    public int ExBMonth { get; set; }
    [Required]
    [Range(1,3000)]
    public int ExBYear { get; set; }
    [Required]
    public int UserID { get; set; }

    public virtual ExpensesBook ExpensesBook { get; set; }
}