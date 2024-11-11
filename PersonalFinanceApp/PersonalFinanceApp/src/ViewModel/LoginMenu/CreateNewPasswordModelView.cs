using System.Windows.Input;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class CreateNewPasswordModelView : BaseViewModel
{
    public ICommand NavigationConfirmNewPassword { get; set; }

    public CreateNewPasswordModelView(LoginNavigationStore navigationStore)
    {
        NavigationConfirmNewPassword = new CreateNewPasswordCommand(navigationStore);
    }
}