using PersonalFinanceApp.ViewModel.LoginMenu;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.Command;

public class ConfirmNewPasswordCommand : BaseCommand
{
    private readonly LoginNavigationStore _navigationStore;

    public ConfirmNewPasswordCommand(LoginNavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
    }
    public override void Execute(object parameter)
    {
        _navigationStore.CurrentViewModel = new CreateNewPasswordModelView(_navigationStore);
    }
}