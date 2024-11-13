using System.Windows.Input;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class ResetPasswordViewModel : BaseViewModel
{
    public ICommand NavigateConfirmEmailCommand { get; set; }

    public ResetPasswordViewModel(NavigationStore navigationStore)
    {
        NavigateConfirmEmailCommand = new NavigateCommand<CodeVerificationViewModel>(
            navigationStore,
            () => new CodeVerificationViewModel(navigationStore),
            VerifyEmail
        );
    }

    public bool VerifyEmail()
    {
        return true;
    }
}