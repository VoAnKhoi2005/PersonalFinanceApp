using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class LoginNewAccountViewModel : BaseViewModel
{
    #region Properties

    public bool IncorrectPasswordUserName { get; set; } = false;
    public bool InCorrectName { get; set; } = false;
    public bool InCorrectGmail { get; set; } = false;
    public bool InCorrectPassword { get; set; } = false;
    public bool InCorrectPasswordConfirm { get; set; } = false;

    private string _userNameLogin= string.Empty;
    public string UserNameLogin
    {
        get => _userNameLogin;
        set
        {
            _userNameLogin = value;
            OnPropertyChanged();
        }
    }

    private string _passwordLogin = string.Empty;
    public string PasswordLogin
    {
        get => _passwordLogin;
        set
        {
            _passwordLogin = value;
            OnPropertyChanged();
        }
    }
    private string _userNameNewAccount;
    public string UserNameNewAccount {
        get => _userNameNewAccount;
        set {
            _userNameNewAccount = value;
            OnPropertyChanged();
        }
    }
    private string _passwordNewAccount;
    public string PasswordNewAccount {
        get => _passwordNewAccount;
        set {
            _passwordNewAccount = value;
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
    public ICommand PasswordLoginChangedCommand { get; set; }
    public ICommand PasswordNewAccountChangedCommand { get; set; }
    public ICommand PasswordConfirmChangedCommand { get; set; }
    public ICommand FocusLoginCommand { get; set; }
    public ICommand FocusNewAccountCommand {  get; set; }
    public ICommand ClearPasswordNewAccountCommand { get; set; }
    public ICommand ClearPasswordNewAccountConfirmCommand { get; set; }
    public ICommand ClearPasswordLoginCommand { get; set; }

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

        PasswordLoginChangedCommand = new RelayCommand<PasswordBox>((p)  => true, (p) => { PasswordLogin = p.Password; });
        PasswordConfirmChangedCommand = new RelayCommand<PasswordBox>((p)  => true, (p) => { PasswordConfirm = p.Password; });
        FocusLoginCommand = new RelayCommand<TabItem>((p) => { return true; }, (p) => { ClearText(p); });
        FocusNewAccountCommand = new RelayCommand<TabItem>((p) => { return true; }, (p) => { ClearText(p); });
        ClearPasswordLoginCommand = new RelayCommand<PasswordBox>(p => { return true; }, (p) => { ClearPassword(p); });
        ClearPasswordNewAccountCommand = new RelayCommand<PasswordBox>(p => { return true; }, (p) => { ClearPassword(p); });
        ClearPasswordNewAccountConfirmCommand = new RelayCommand<PasswordBox>(p => { return true; }, (p) => { ClearPassword(p); });
    }

    private void LoginSuccess(User loginUser)
    {
        loginUser = DBManager.GetFirst<User>(u => u.Username == UserNameLogin);
        MainWindow mainWindow = new MainWindow(loginUser);
        if (Application.Current.MainWindow != null) 
            Application.Current.MainWindow.Close();
        Application.Current.MainWindow = mainWindow;
        mainWindow.Show();
    }

    private bool VerifyLogin(User? loginUser)
    {
        loginUser = DBManager.GetFirst<User>(u => u.Username == UserNameLogin);
        if (loginUser == null) {
            IncorrectPasswordUserName = true;
            return false;
        }
        return loginUser.VerifyPassword(PasswordLogin);
    }
    private void ClearText(object parameter) {
        TabItem tab = parameter as TabItem;
        if (tab != null) {
            switch (tab.Name) {
                case "LoginTab":
                    UserNameNewAccount = string.Empty;
                    Gmail = string.Empty;
                    break;
                case "NewAccountTab":
                    UserNameLogin = string.Empty;
                    break;
                default:
                    break;
            }
        }
    }
    private void ClearPassword(object parameter) {
        PasswordBox p = parameter as PasswordBox;
        if (p != null) {
            p.Password = "";
        }
    }
}