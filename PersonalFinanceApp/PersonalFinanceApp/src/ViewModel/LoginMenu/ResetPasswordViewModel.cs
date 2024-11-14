using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class ResetPasswordViewModel : BaseViewModel
{
    #region Properties
    public bool IncorrectUserGmail { get; set; } = false;
    private string _userNameReset;
    public string UserNameReset {
        get => _userNameReset;
        set {
            _userNameReset = value;
            OnPropertyChanged();
        }
    }
    private string _gmailReset;
    public string GmailReset {
        get => _gmailReset;
        set {
            _gmailReset = value;
            OnPropertyChanged();
        }
    }
    #endregion
    public ICommand NavigateConfirmEmailCommand { get; set; }

    public ResetPasswordViewModel(IServiceProvider serviceProvider)
    {
        NavigateConfirmEmailCommand = new NavigateCommand<CodeVerificationViewModel>(
            serviceProvider.GetRequiredService<NavigationStore>(),
            () => serviceProvider.GetRequiredService<CodeVerificationViewModel>(),
            VerifyEmail
        );
    }

    public bool VerifyEmail()
    {
        return true;
    }
}