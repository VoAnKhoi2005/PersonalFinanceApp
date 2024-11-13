using System.Windows.Controls;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class MainViewModel : BaseViewModel
{

    public DashboardViewModel DashboardViewModel { get; set; }
    public GoalplanViewModel GoalplanViewModel { get; set; }
    public SummaryViewModel SummaryViewModel { get; set; }

    public MainViewModel()
    {
        DashboardViewModel = new DashboardViewModel();
        GoalplanViewModel = new GoalplanViewModel();
        SummaryViewModel = new SummaryViewModel();
    }
}