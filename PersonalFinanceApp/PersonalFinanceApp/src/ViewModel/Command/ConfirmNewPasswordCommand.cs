using PersonalFinanceApp.ViewModel.LoginMenu;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.Command;

public class ConfirmNewPasswordCommand : BaseCommand
{
    private readonly LoginNavigationStore _navigationStore;
    private readonly CreateNewPasswordModelView _modelView;

    public ConfirmNewPasswordCommand(LoginNavigationStore navigationStore, CreateNewPasswordModelView modelView)
    {
        _navigationStore = navigationStore;
        _modelView = modelView;
    }
    public override void Execute(object parameter)
    {
        _navigationStore.CurrentViewModel = new LoginNewAccountModelView(_navigationStore);
    }

    public override bool CanExecute(object parameter) => _modelView.VerifyNewPassword();
}