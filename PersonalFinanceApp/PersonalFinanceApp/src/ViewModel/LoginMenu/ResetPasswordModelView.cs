using System.Windows.Input;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class ResetPasswordModelView : BaseViewModel
{
    public ICommand NavigateConfirmEmailCommand { get; set; }

    public ResetPasswordModelView(LoginNavigationStore navigationStore)
    {
        NavigateConfirmEmailCommand = new ConfirmEmailCommand(navigationStore, this);
    }

    public bool VerifyEmail()
    {
        return true;
    }
}