using PersonalFinanceApp.ViewModel.LoginMenu;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.Command;

public class ForgotPasswordCommand : BaseCommand
{
    private readonly LoginNavigationStore _navigationStore;

    public ForgotPasswordCommand(LoginNavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
    }

    public override void Execute(object parameter)
    {
        _navigationStore.CurrentViewModel = new ResetPasswordModelView(_navigationStore);
    }
}