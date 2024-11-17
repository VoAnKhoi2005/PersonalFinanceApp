using System.Windows.Input;
using PersonalFinanceApp.ViewModel.Command;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class GoalplanViewModel : BaseViewModel
{
    public ICommand AddNewGoalCommand { get; set; }

    public GoalplanViewModel(IServiceProvider serviceProvider)
    {
        AddNewGoalCommand = new NavigateModalCommand<GoalplanAddNewViewModel>(serviceProvider);
    }
}