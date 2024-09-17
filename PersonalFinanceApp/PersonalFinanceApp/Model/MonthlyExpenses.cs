namespace PersonalFinanceApp.Model
{
    public class MonthlyExpenses
    {
        private int _month = 1;
        private int _year = 0;
        private decimal _limit = decimal.MaxValue;

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

        public decimal Limit
        {
            get => _limit;
            set
            {
                if (value < 0)
                    _limit = 0;
                else
                    _limit = value;
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