using PersonalFinanceApp.ViewModel.LoginMenu;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.Command;

public class ConfirmCodeCommand : BaseCommand
{
    private readonly LoginNavigationStore _navigationStore;
    private readonly CodeVerificationModelView _codeVerificationMV;

    public ConfirmCodeCommand(LoginNavigationStore navigationStore, CodeVerificationModelView codeVMV)
    {
        _navigationStore = navigationStore;
        _codeVerificationMV = codeVMV;
    }

    public override void Execute(object parameter)
    {
        _navigationStore.CurrentViewModel = new CreateNewPasswordModelView(_navigationStore);
    }

    public override bool CanExecute(object parameter) => _codeVerificationMV.VerifyCode();
}