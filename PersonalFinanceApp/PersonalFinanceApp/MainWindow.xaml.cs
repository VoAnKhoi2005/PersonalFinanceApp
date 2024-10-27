using System.Windows;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;

namespace PersonalFinanceApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
<<<<<<< Updated upstream
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
=======
            DBManager.GetCondition<Expense>(ex => ex.Amount > 10000000);
>>>>>>> Stashed changes
        }
    }
}