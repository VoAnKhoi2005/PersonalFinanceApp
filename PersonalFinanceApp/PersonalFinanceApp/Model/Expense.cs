namespace PersonalFinanceApp.Model
{
    public class Expense
    {
        public readonly int ID;
        public float Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string CategoryName { get; set; }

    }
}
