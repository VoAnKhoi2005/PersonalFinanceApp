using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceApp.Model
{
    public class Expense
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Range(1,1000000000000)]
        public decimal Amount { get; set; }

        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public bool Recurring { get; set; }

        public DateOnly RecurringDate { get; set; }


        [Required]
        public string CategoryID { get; set; }
        public virtual Category Category { get; set; }

        [Required]
        public string ExpensesBookID { get; set; }
        public virtual ExpensesBook ExpensesBook { get; set; }
    }
}