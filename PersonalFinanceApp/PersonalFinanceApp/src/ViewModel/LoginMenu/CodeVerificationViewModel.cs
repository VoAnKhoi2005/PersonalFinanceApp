using System.Windows.Input;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class CodeVerificationViewModel : BaseViewModel
{
    public ICommand NavigationConfirmCodeCommand { get; set; }

    public CodeVerificationViewModel(NavigationStore navigationStore)
    {
        NavigationConfirmCodeCommand = new NavigateCommand<CreateNewPasswordViewModel>(
            navigationStore,
            () => new CreateNewPasswordViewModel(navigationStore),
            VerifyCode
            );
    }

    public bool VerifyCode()
    {
        return true;
    }
}