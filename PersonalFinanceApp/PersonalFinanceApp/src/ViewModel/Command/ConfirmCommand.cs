using PersonalFinanceApp.ViewModel.LoginMenu;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.Command;

public class ConfirmCommand : BaseCommand
{
    private readonly LoginNavigationStore _navigationStore;

    public ConfirmCommand(LoginNavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
    }

    public override void Execute(object parameter)
    {
        _navigationStore.CurrentViewModel = new CodeVerificationModelView(_navigationStore);
    }
}