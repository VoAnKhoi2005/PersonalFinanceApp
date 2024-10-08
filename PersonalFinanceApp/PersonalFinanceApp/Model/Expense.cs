using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model
{
    public class Expense
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Range(1,1000000000000)]
        public long Amount { get; set; }

        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public bool Recurring { get; set; }

        public DateOnly? RecurringDate { get; set; }

        //Relationship
        [Required]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
        [Required]
        [Range(1,12)]
        public int ExBMonth { get; set; }

        [Required]
        [Range(1,3000)]
        public int ExBYear { get; set; }
        public virtual ExpensesBook ExpensesBook { get; set; }
    }
}