using System.Windows.Input;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class CreateNewPasswordViewModel : BaseViewModel
{
    public ICommand NavigationConfirmNewPassword { get; set; }

    public CreateNewPasswordViewModel(NavigationStore navigationStore)
    {
        NavigationConfirmNewPassword = new NavigateCommand<LoginNewAccountViewModel>(
            navigationStore,
            () => new LoginNewAccountViewModel(navigationStore),
            VerifyNewPassword
            );
    }

    public bool VerifyNewPassword()
    {
        return true;
    }
}