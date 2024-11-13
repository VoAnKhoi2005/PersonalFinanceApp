using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class LoginNewAccountModelView : BaseViewModel
{
    #region Properties

    public bool IncorrectPasswordUserName { get; set; } = false;
    public bool InCorrectName { get; set; } = false;
    public bool InCorrectGmail { get; set; } = false;
    public bool InCorrectPassword { get; set; } = false;
    public bool InCorrectPasswordConfirm { get; set; } = false;

    private string _userNameLogin;
    public string UserNameLogin
    {
        get => _userNameLogin;
        set
        {
            _userNameLogin = value;
            OnPropertyChanged();
        }
    }

    private string _passwordLogin;
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
    private string _passwordConirm;
    public string PasswordConfirm {
        get => _passwordConirm;
        set {
            _passwordConirm = value;
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

    #endregion

    #region Command
    public ICommand ForgotPasswordCommand { get; set; }
    public ICommand PasswordLoginChangedCommand { get; set; }
    public ICommand PasswordNewAccountChangedCommand { get; set; }
    public ICommand PasswordConfirmChangedCommand { get; set; }
    public ICommand FocusLoginCommand { get; set; }
    public ICommand FocusNewAccountCommand {  get; set; }
    public ICommand ClearPasswordNewAccountCommand { get; set; }
    public ICommand ClearPasswordNewAccountConfirmCommand { get; set; }
    public ICommand ClearPasswordLoginCommand { get; set; }

    private readonly LoginNavigationStore _navigationStore;

    #endregion

    public LoginNewAccountModelView(LoginNavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        ForgotPasswordCommand = new ForgotPasswordCommand(navigationStore);
        PasswordLoginChangedCommand = new RelayCommand<PasswordBox>((p)  => {return true;}, (p) => { PasswordLogin = p.Password; });
        PasswordNewAccountChangedCommand = new RelayCommand<PasswordBox>((p)  => {return true;}, (p) => { PasswordNewAccount = p.Password; });
        PasswordConfirmChangedCommand = new RelayCommand<PasswordBox>((p)  => {return true;}, (p) => { PasswordConfirm = p.Password; });
        FocusLoginCommand = new RelayCommand<TabItem>((p) => { return true; }, (p) => { ClearText(p); });
        FocusNewAccountCommand = new RelayCommand<TabItem>((p) => { return true; }, (p) => { ClearText(p); });
        ClearPasswordLoginCommand = new RelayCommand<PasswordBox>(p => { return true; }, (p) => { ClearPassword(p); });
        ClearPasswordNewAccountCommand = new RelayCommand<PasswordBox>(p => { return true; }, (p) => { ClearPassword(p); });
        ClearPasswordNewAccountConfirmCommand = new RelayCommand<PasswordBox>(p => { return true; }, (p) => { ClearPassword(p); });
    }

    private void LoginSuccess(object parameter)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        if (parameter is Window currentWindow)
        {
            currentWindow.Close();
        }
    }

    private bool VerifyLogin(object parameter)
    {
        var loginUser = DBManager.GetFirst<Model.User>(u => u.Username == UserNameLogin);
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