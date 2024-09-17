namespace PersonalFinanceApp.Model
{
    public class Expense
    {
        public readonly int ID;
        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set
            {
                if (value < 0)
                    _amount = 0;
                else
                    _amount = value;
            }
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string CategoryName { get; set; }
    }
}