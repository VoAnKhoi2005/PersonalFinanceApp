namespace PersonalFinanceApp.Model
{
    public class Manager
    {
        #region Singleton
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
        #endregion Singleton

        public decimal Saving { get; }
        public decimal Goal { get; set; }
        private List<MonthlyExpenses> AllTimeExpensesList;
        public MonthlyExpenses CurrentMonthlyExpenses { get; }

        private Manager()
        {
            AllTimeExpensesList = new List<MonthlyExpenses>();
        }
    }
}
