using System.Windows.Input;
using PersonalFinanceApp.ViewModel.Command;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class GoalplanViewModel : BaseViewModel
{
    public ICommand testCommand { get; set; }

    public GoalplanViewModel(IServiceProvider serviceProvider)
    {
        testCommand = new TestCommand();
    }
}