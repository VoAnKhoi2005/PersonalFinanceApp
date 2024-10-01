using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceApp.Model
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MinLength(1)]
        public string Name { get; set; }

        public virtual List<Expense> Expenses { get; set; }

        //Relationship
        [Required]
        public string ExpensesBookID { get; set; }
        public virtual ExpensesBook ExpensesBook { get; set; }
    }
}
