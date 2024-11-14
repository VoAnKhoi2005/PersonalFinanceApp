using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class CreateNewPasswordViewModel : BaseViewModel {
    #region Properties
    private bool _incorrectPasswordReset = false;
    public bool IncorrectPasswordReset {
        get => _incorrectPasswordReset;
        set {
            _incorrectPasswordReset = value;
            OnPropertyChanged();
        }
    }
    private bool _incorrectConfirmReset = false;
    public bool IncorrectConfirmReset {
        get => _incorrectConfirmReset;
        set {
            _incorrectConfirmReset = value;
            OnPropertyChanged();
        }
    }

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
    public ICommand CheckMathConfirmPasswordNewCommand { get; set; }
    public ICommand CheckFormatPassowrdNewCommand { get; set; }
    public CreateNewPasswordViewModel(NavigationStore navigationStore) {

        NavigationConfirmNewPassword = new NavigateCommand<LoginNewAccountViewModel>(
            serviceProvider.GetRequiredService<NavigationStore>(),
            () => serviceProvider.GetRequiredService<LoginNewAccountViewModel>(),
            VerifyNewPassword
            );
        PasswordResetConfirmChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { PasswordResetConfirm = p.Password; });
        CheckMathConfirmPasswordNewCommand = new RelayCommand<PasswordBox>(p => { return true; }, (p) => { CheckFormat(p); });
        CheckFormatPassowrdNewCommand = new RelayCommand<TextBox>(p => { return true; }, (p) => { CheckFormat(p); });
    }

    public bool VerifyNewPassword() {
        return true;
    }
    private void CheckFormat(object parameter) {
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        if (parameter is TextBox) {
            TextBox p = parameter as TextBox;
            if (Regex.IsMatch(p.Text, pattern)) {
                IncorrectPasswordReset = false;
            }
            else IncorrectPasswordReset = true;
        }
        else {
            if (PasswordReset == PasswordResetConfirm) {
                IncorrectConfirmReset = false;
            }
            else IncorrectConfirmReset = true;
        }
    }
}