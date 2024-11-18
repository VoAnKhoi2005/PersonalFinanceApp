using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class CreateNewPasswordViewModel : BaseViewModel {
    private readonly IServiceProvider _serviceProvider;
    private readonly SharedDataService _sharedDataService;
    #region Properties
    public string name;
    public bool correct = false;
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

    #region Command
    public ICommand NavigationConfirmNewPassword { get; set; }
    public ICommand PasswordResetConfirmChangedCommand { get; set; }
    public ICommand CheckMathConfirmPasswordNewCommand { get; set; }
    public ICommand CheckFormatPassowrdNewCommand { get; set; }
    #endregion
    public CreateNewPasswordViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _sharedDataService = serviceProvider.GetRequiredService<SharedDataService>();
        NavigationConfirmNewPassword = new NavigateCommand<LoginNewAccountViewModel>(serviceProvider, VerifyNewPassword);
        //Password Binding
        PasswordResetConfirmChangedCommand = new RelayCommand<PasswordBox>( p => PasswordResetConfirm = p.Password);
        //Format
        CheckMathConfirmPasswordNewCommand = new RelayCommand<PasswordBox>( p => CheckFormat(p));
        CheckFormatPassowrdNewCommand = new RelayCommand<TextBox>( p => CheckFormat(p));
    }

    public bool VerifyNewPassword() {
        name = _sharedDataService.SharedList[0];
        if (correct) {
            var usr = DBManager.GetFirst<User>(u => u.Username == name);
            if(usr.ChangePassword(PasswordReset)) return true;
        }
        return false;
    }
    private void CheckFormat(object parameter) {
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        if (parameter is TextBox) {
            TextBox p = parameter as TextBox;
            if (Regex.IsMatch(p.Text, pattern)) {
                correct = true;
                IncorrectPasswordReset = false;
            }
            else {
                correct = false;
                IncorrectPasswordReset = true;
            }
        }
        else {
            if (PasswordReset == PasswordResetConfirm) {
                correct = true;
                IncorrectConfirmReset = false;
            }
            else {
                correct = false;
                IncorrectConfirmReset = true;
            }
        }
    }
}