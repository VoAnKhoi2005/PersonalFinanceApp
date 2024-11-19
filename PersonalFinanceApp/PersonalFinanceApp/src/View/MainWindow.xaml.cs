using System.Windows;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel;
using PersonalFinanceApp.ViewModel.MainMenu;

namespace PersonalFinanceApp.View;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        Closed += OnWindowClosed;
        InitializeComponent();
    }

    public MainWindow(BaseViewModel datacontext, User curUser)
    {
        Closed += OnWindowClosed;
        DataContext = datacontext;
        InitializeComponent();
    }

    private void OnWindowClosed(object? sender, EventArgs e)
    {
        if (DataContext is BaseViewModel disposableViewModel)
        {
            disposableViewModel.Dispose();
        }
    }
}