using PersonalFinanceApp.ViewModel.LoginMenu;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.Command;

public class ConfirmEmailCommand : BaseCommand
{
    private readonly LoginNavigationStore _navigationStore;
    private readonly ResetPasswordModelView resetPasswordVM;

    public ConfirmEmailCommand(LoginNavigationStore navigationStore, ResetPasswordModelView resetPasswordModelView)
    {
        _navigationStore = navigationStore;
        resetPasswordVM = resetPasswordModelView;
    }

    public override void Execute(object parameter)
    {
        _navigationStore.CurrentViewModel = new CodeVerificationModelView(_navigationStore);
    }

    public override bool CanExecute(object parameter) => resetPasswordVM.VerifyEmail();
}