using System.Windows;
using PersonalFinanceApp.ViewModel;

namespace PersonalFinanceApp.View;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        Closed += OnWindowClosed;
    }

    private void OnWindowClosed(object? sender, EventArgs e)
    {
        if (DataContext is BaseViewModel disposableViewModel)
        {
            disposableViewModel.Dispose();
        }
    }
}