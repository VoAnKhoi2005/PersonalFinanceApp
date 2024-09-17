namespace PersonalFinanceApp.Model
{
    public class Manager
    {
        private static Manager _instance = null;
        public static Manager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Manager();
                return _instance;
            }
        }

        private Manager()
        {
            AllTimeExpensesList = new List<MonthlyExpenses>();
        }

        private List<MonthlyExpenses> AllTimeExpensesList;
        public MonthlyExpenses CurrentMonthlyExpenses { get; }
    }
}
