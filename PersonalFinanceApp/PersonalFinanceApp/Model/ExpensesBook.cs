using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceApp.Model
{
    public class ExpensesBook
    {
        [Key]
        public string ID { get; set; }

        [Required]
        [Range(1,12)]
        private int Month { get; set; }

        [Required]
        [Range(1,3000)]
        private int Year { get; set; }

        [Range(0, 10000000000000)]
        private decimal Budget { get; set; }

        [Range(0, 10000000000000)]
        public decimal Spending { get; set; }
        
        public string UserID { get; set; }
        public virtual User User { get; set; }

        public virtual List<Expense> Expenses { get; set; } = new List<Expense>();
        public virtual List<Category> Categories { get; set; } = new List<Category>();
    }
}