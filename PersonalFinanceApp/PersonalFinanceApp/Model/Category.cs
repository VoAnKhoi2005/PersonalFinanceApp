namespace PersonalFinanceApp.Model
{
    public class Category
    {
        public readonly int ID;
        public string Name { get; set; }
        public List<Expense> Expenses { get; set; }
    }
}
