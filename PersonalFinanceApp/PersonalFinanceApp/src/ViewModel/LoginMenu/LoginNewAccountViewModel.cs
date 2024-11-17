using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.etc;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.View;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class LoginNewAccountViewModel : BaseViewModel {
    private readonly IServiceProvider _serviceProvider;

    #region Properties
    private bool _incorrectPasswordUserName = false;
    public bool IncorrectPasswordUserName {
        get => _incorrectPasswordUserName;
        set {
            _incorrectPasswordUserName = value;
            OnPropertyChanged();
        }
    }

    private bool _incorrectName = false;
    public bool InCorrectName {
        get => _incorrectName;
        set {
            _incorrectName = value;
            OnPropertyChanged();
        }
    }

    private bool _incorrectEmail = false;
    public bool InCorrectGmail {
        get => _incorrectEmail;
        set {
            _incorrectEmail = value;
            OnPropertyChanged();
        }
    }
    private bool _inCorrectPassword = false;
    public bool InCorrectPassword {
        get => _inCorrectPassword;
        set {
            _inCorrectPassword = value;
            OnPropertyChanged();
        }
    }
    private bool _inCorrectPasswordConfirm = false;
    public bool InCorrectPasswordConfirm {
        get => _inCorrectPasswordConfirm;
        set {
            _inCorrectPasswordConfirm = value;
            OnPropertyChanged();
        }
    }

    private string _userNameLogin = string.Empty;
    public string UserNameLogin {

        get => _userNameLogin;
        set {
            _userNameLogin = value;
            OnPropertyChanged();
        }
    }

    private string _passwordLogin = string.Empty;

    public string PasswordLogin
    {
        get => _passwordLogin;
        set {
            _passwordLogin = value;
            OnPropertyChanged();
        }
    }

    private string _userNameNewAccount;

    public string UserNameNewAccount
    {
        get => _userNameNewAccount;
        set
        {
            _userNameNewAccount = value;
            OnPropertyChanged();
        }
    }

    private string _passwordNewAccount;

    public string PasswordNewAccount
    {
        get => _passwordNewAccount;
        set
        {
            _passwordNewAccount = value;
            OnPropertyChanged();
        }
    }

    private string _passwordConfirm;

    public string PasswordConfirm
    {
        get => _passwordConfirm;
        set
        {
            _passwordConfirm = value;
            OnPropertyChanged();
        }
    }

    private string _gmail;
    public string Gmail
    {
        get => _gmail;
        set
        {
            _gmail = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region Command
    public ICommand LoginCommand { get; set; }
    public ICommand CreateAccountCommand {  get; set; }
    public ICommand ForgotPasswordCommand { get; set; }
    public ICommand PasswordLoginChangedCommand { get; set; }
    public ICommand PasswordNewAccountChangedCommand { get; set; }
    public ICommand PasswordConfirmChangedCommand { get; set; }
    public ICommand FocusLoginCommand { get; set; }
    public ICommand FocusNewAccountCommand { get; set; }
    public ICommand ClearPasswordNewAccountCommand { get; set; }
    public ICommand ClearPasswordNewAccountConfirmCommand { get; set; }
    public ICommand ClearPasswordLoginCommand { get; set; }
    public ICommand CheckFormatUserNameNewAccountCommand { get; set; }
    public ICommand CheckFormatGmailCommand { get; set; }
    public ICommand CheckFormatPasswordNewAccountCommand { get; set; }
    public ICommand CheckMathConfirmPasswordCommand { get; set; }

    #endregion

    public LoginNewAccountViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        ForgotPasswordCommand = new NavigateCommand<ResetPasswordViewModel>(serviceProvider);

        LoginCommand = new RelayCommand<User>(LoginSuccess, VerifyLogin);
        // LoginCommand = new RelayCommand<User>(
        //     p => { return true; },
        //     p => { LoginSuccess(p); }
        //     );
        //Set up Password
        PasswordLoginChangedCommand = new RelayCommand<PasswordBox>( (p) => { PasswordLogin = p.Password; });
        PasswordNewAccountChangedCommand = new RelayCommand<PasswordBox>( (p) => { PasswordNewAccount = p.Password; });
        PasswordConfirmChangedCommand = new RelayCommand<PasswordBox>( (p) => { PasswordConfirm = p.Password; });
        //Clear text
        FocusLoginCommand = new RelayCommand<TabItem>( (p) => { ClearText(p); });
        FocusNewAccountCommand = new RelayCommand<TabItem>( (p) => { ClearText(p); });
        ClearPasswordLoginCommand = new RelayCommand<PasswordBox>( (p) => { ClearPassword(p); });
        ClearPasswordNewAccountCommand = new RelayCommand<PasswordBox>( (p) => { ClearPassword(p); });
        ClearPasswordNewAccountConfirmCommand = new RelayCommand<PasswordBox>( (p) => { ClearPassword(p); });
        //Format
        CheckFormatUserNameNewAccountCommand = new RelayCommand<TextBox>((p) => { Format(p); });
        CheckFormatGmailCommand = new RelayCommand<TextBox>((p) => { Format(p); });
        CheckFormatPasswordNewAccountCommand = new RelayCommand<PasswordBox>((p) => { Format(p); });
        CheckMathConfirmPasswordCommand = new RelayCommand<PasswordBox>((p) => { MatchPassword(p); });
    }

    private void LoginSuccess(User loginUser) {

        if (VerifyLogin(loginUser)) {

            IncorrectPasswordUserName = false;

            loginUser = DBManager.GetFirst<User>(u => u.Username == UserNameLogin);

            var factory = _serviceProvider.GetRequiredService<IWindowFactory>();
            MainWindow mainWindow = factory.CreateMainWindow(loginUser);

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Close();

            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
        }
        else {
            IncorrectPasswordUserName = true;
        }

    }

    private bool VerifyLogin(User? loginUser) {

        loginUser = DBManager.GetFirst<User>(u => u.Username == UserNameLogin);
        if (loginUser == null) {
            IncorrectPasswordUserName = true;
            return false;
        }

        return loginUser.VerifyPassword(PasswordLogin);
    }

    private void ClearText(object parameter)
    {
        TabItem tab = parameter as TabItem;
        if (tab != null)
        {
            switch (tab.Name)
            {
                case "LoginTab":
                    UserNameNewAccount = string.Empty;
                    Gmail = string.Empty;
                    InCorrectName = false;
                    InCorrectGmail = false;
                    InCorrectPassword = false;
                    InCorrectPasswordConfirm = false;
                    break;
                case "NewAccountTab":
                    UserNameLogin = string.Empty;
                    break;
            }
        }
    }

    private void ClearPassword(object parameter)
    {
        PasswordBox p = parameter as PasswordBox;
        if (p != null)
        {
            p.Password = string.Empty;
        }
    }

    private void Format(object parameter) {
        TextBox p = parameter as TextBox;
        string pattern;
        if (p != null) {
            if (p.Name.CompareTo("UserNameNewAccount") == 0) {
                pattern = @"^[a-zA-Z0-9_]{8,}$";
                if (Regex.IsMatch(p.Text, pattern)) {
                    InCorrectName = false;
                } else InCorrectName = true;
            } else if (p.Name.CompareTo("Gmail") == 0) {
                pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
                if (Regex.IsMatch(p.Text, pattern)) {
                    InCorrectGmail = false;
                }
                else InCorrectGmail = true;
            }
            
        } else if (parameter is PasswordBox) {
            PasswordBox pb = parameter as PasswordBox;
            pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            if (Regex.IsMatch(pb.Password, pattern) ) {
                InCorrectPassword = false;
            }   
            else InCorrectPassword = true;
        }
    }
    private void MatchPassword(object parameter) {
        if (PasswordNewAccount == PasswordConfirm) {
            InCorrectPasswordConfirm = false;
        }
        else InCorrectPasswordConfirm = true;
    }
}