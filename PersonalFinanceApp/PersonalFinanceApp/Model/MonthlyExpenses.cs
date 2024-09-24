namespace PersonalFinanceApp.Model
{
    public class MonthlyExpenses
    {
        private int _month = 1;
        public int Month
        {
            get => _month;
            set
            {
                if (value < 0)
                    _month = 0;
                else
                    _month = value;
            }
        }


        private int _year = 0;
        public int Year
        {
            get => _year;
            set
            {
                if (_year < 0)
                    _year = 0;
                else
                    _year = value;
            }
        }

        private decimal _budget = decimal.MaxValue;
        public decimal Budget
        {
            get => _budget;
            set
            {
                if (value < 0)
                    _budget = 0;
                else
                    _budget = value;
            }
        }

        private Dictionary<DateTime, List<Expense>> Expenses;
        public List<Category> Categories { get; }

        public MonthlyExpenses()
        {
            Categories = new List<Category>();
            Expenses = new Dictionary<DateTime, List<Expense>>();
        }
    }
}