using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class LoginNewAccountViewModel : BaseViewModel
{
    #region Properties

    public bool IsLogin { get; set; } = false;
    public bool IsIncorrect { get; set; } = false;
    public bool IsCorrectName { get; set; } = false;
    public bool IsCorrectGmail { get; set; } = false;
    public bool IsCorrectPassword { get; set; } = false;
    public bool IsCorrectPasswordConfirm { get; set; } = false;

    private string _userName = string.Empty;
    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            OnPropertyChanged();
        }
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }
    private string _passwordConfirm;
    public string PasswordConfirm {
        get => _passwordConfirm;
        set {
            _passwordConfirm = value;
            OnPropertyChanged();
        }
    }
    private string _gmail;
    public string Gmail {
        get => _gmail;
        set {
            _gmail = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoginCommand { get; set; }
    public ICommand ForgotPasswordCommand { get; set; }
    public ICommand PasswordChangedCommand { get; set; }
    public ICommand PasswordConfirmChangedCommand { get; set; }

    #endregion

    public LoginNewAccountViewModel(NavigationStore navigationStore)
    {
        ForgotPasswordCommand = new NavigateCommand<ResetPasswordViewModel>(
            navigationStore,
            () => new ResetPasswordViewModel(navigationStore)
        );

        LoginCommand = new RelayCommand<User>(
            canExecute: VerifyLogin,
            execute: LoginSuccess
            );

        PasswordChangedCommand = new RelayCommand<PasswordBox>((p)  => true, (p) => { Password = p.Password; });
        PasswordConfirmChangedCommand = new RelayCommand<PasswordBox>((p)  => true, (p) => { PasswordConfirm = p.Password; });
    }

    private void LoginSuccess(User loginUser)
    {
        loginUser = DBManager.GetFirst<User>(u => u.Username == UserName);
        MainWindow mainWindow = new MainWindow(loginUser);
        if (Application.Current.MainWindow != null) 
            Application.Current.MainWindow.Close();
        Application.Current.MainWindow = mainWindow;
        mainWindow.Show();
    }

    private bool VerifyLogin(User? loginUser)
    {
        loginUser = DBManager.GetFirst<User>(u => u.Username == UserName);
        if (loginUser == null) {
            IsIncorrect = true;
            return false;
        }
        return loginUser.VerifyPassword(Password);
    }
}