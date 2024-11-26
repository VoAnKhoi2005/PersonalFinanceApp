using System.Windows;
using System.Windows.Controls.Primitives;
using PersonalFinanceApp.Database;
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
        GoalCategory goalCategory = new GoalCategory();
        List<string> items = new List<string>() {
            "<New>",
            "New vehicle",
            "New home",
            "Hoiliday trip",
            "Education",
            "Emergency fund",
            "Health care",
            "Party",
            "Kids spoiling",
            "Charity",
        };
        foreach (var item in items) {
            goalCategory.Name = item;
            DBManager.Insert(goalCategory);
        }
    }

    private void OnWindowClosed(object? sender, EventArgs e)
    {
        if (DataContext is BaseViewModel disposableViewModel)
        {
            disposableViewModel.Dispose();
        }
    }
}