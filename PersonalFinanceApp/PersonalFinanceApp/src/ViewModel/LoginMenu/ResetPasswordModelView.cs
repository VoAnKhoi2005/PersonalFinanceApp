using System.Windows.Input;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class ResetPasswordModelView : BaseViewModel
{
    public ICommand NavigateConfirmCommand { get; set; }

    public ResetPasswordModelView(LoginNavigationStore navigationStore)
    {
        NavigateConfirmCommand = new ConfirmCommand(navigationStore);
    }
}