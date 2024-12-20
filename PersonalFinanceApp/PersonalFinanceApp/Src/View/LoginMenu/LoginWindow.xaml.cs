using System.Windows;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel;

namespace PersonalFinanceApp.View;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        Closed += OnWindowClosed;
        User newUser = new User("admin", "pass", "admin@123");
        newUser.DefaultBudget = 100000;
        DBManager.Insert(newUser);
        this.Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/img/f7e028af-f53f-47b3-91cc-fd1e3a553db1.ico"));
    }

    private void OnWindowClosed(object? sender, EventArgs e)
    {
        if (DataContext is BaseViewModel disposableViewModel)
        {
            disposableViewModel.Dispose();
        }
    }
}