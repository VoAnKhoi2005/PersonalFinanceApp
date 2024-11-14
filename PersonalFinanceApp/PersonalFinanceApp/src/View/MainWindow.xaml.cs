using System.Windows;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel;
using PersonalFinanceApp.ViewModel.MainMenu;

namespace PersonalFinanceApp.View;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(BaseViewModel datacontext, User curUser)
    {
        DataContext = datacontext;
        InitializeComponent();
    }
}