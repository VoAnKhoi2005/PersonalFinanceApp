using PersonalFinanceApp.ViewModel.LoginMenu;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.Command;

public class CreateNewPasswordCommand : BaseCommand
{
    private readonly LoginNavigationStore _navigationStore;

    public CreateNewPasswordCommand(LoginNavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
    }
    public override void Execute(object parameter)
    {
        _navigationStore.CurrentViewModel = new LoginNewAccountModelView(_navigationStore);
    }
}