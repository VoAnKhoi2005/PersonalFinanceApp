using System.Windows.Input;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class CodeVerificationModelView : BaseViewModel
{
    public ICommand NavigationConfirmCodeCommand { get; set; }

    public CodeVerificationModelView(LoginNavigationStore navigationStore)
    {
        NavigationConfirmCodeCommand = new ConfirmCodeCommand(navigationStore, this);
    }

    public bool VerifyCode()
    {
        return true;
    }
}