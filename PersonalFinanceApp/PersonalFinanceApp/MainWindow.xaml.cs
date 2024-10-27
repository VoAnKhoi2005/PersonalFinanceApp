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
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }
    }
}