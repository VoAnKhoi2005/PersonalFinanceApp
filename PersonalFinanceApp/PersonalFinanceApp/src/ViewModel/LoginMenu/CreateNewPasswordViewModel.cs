using System.Windows.Controls;
using System.Windows.Input;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class CreateNewPasswordViewModel : BaseViewModel
{
    #region Properties
    public bool IncorrectReset { get; set; } = false;
    private string _passwordReset;
    public string PasswordReset {
        get => _passwordReset;
        set {
            _passwordReset = value;
            OnPropertyChanged();
        }
    }
    private string _passwordResetConfirm;
    public string PasswordResetConfirm {
        get => _passwordResetConfirm;
        set {
            _passwordResetConfirm = value;
            OnPropertyChanged();
        }
    }
    #endregion
    public ICommand NavigationConfirmNewPassword { get; set; }
    public ICommand PasswordResetConfirmChangedCommand { get; set; }

    public CreateNewPasswordViewModel(NavigationStore navigationStore)
    {
        NavigationConfirmNewPassword = new NavigateCommand<LoginNewAccountViewModel>(
            navigationStore,
            () => new LoginNewAccountViewModel(navigationStore),
            VerifyNewPassword
            );
        PasswordResetConfirmChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { PasswordResetConfirm = p.Password; });

    }

    public bool VerifyNewPassword()
    {
        return true;
    }
}