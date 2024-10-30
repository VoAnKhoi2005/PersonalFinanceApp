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
            LoginWindow loginwindow = new LoginWindow();
            loginwindow.ShowDialog();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}