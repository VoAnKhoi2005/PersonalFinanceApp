using System.Windows;
using System.Windows.Controls;
using PersonalFinanceApp.Model;

namespace PersonalFinanceApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(User curUser)
        {
            InitializeComponent();
        }
    }
}