using System.Windows.Input;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class CodeVerificationModelView : BaseViewModel
{
    public ICommand NavigationCreateNewPasswordCommand { get; set; }

    public CodeVerificationModelView(LoginNavigationStore navigationStore)
    {
        NavigationCreateNewPasswordCommand = new CreateNewPasswordCommand(navigationStore);
    }
}