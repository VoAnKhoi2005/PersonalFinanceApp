using System.Windows.Controls;
using System.Windows.Input;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class ResetPasswordModelView : BaseViewModel
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

    #region Command

    public ICommand NavigateConfirmEmailCommand { get; set; }
    #endregion

    public ResetPasswordModelView(LoginNavigationStore navigationStore)
    {
        NavigateConfirmEmailCommand = new ConfirmEmailCommand(navigationStore, this);
    }

    public bool VerifyEmail()
    {
        return true;
    }
}