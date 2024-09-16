namespace PersonalFinanceApp.Model
{
    public class MonthlyExpenses
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public List<Expense> Expenses { get; set; }
        public List<Category> Categories { get; set; }
    }
}