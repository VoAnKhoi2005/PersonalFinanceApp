using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class LoginNewAccountModelView : BaseViewModel
{
    #region Properties

    public bool IsLogin { get; set; } = false;
    public bool IsIncorrect { get; set; } = false;
    public bool IsCorrectName { get; set; } = false;
    public bool IsCorrectGmail { get; set; } = false;
    public bool IsCorrectPassword { get; set; } = false;
    public bool IsCorrectPasswordConfirm { get; set; } = false;

    private string _userName;
    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            OnPropertyChanged();
        }
    }

    private string _password;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
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

    //public ICommand LoginCommand { get; set; }
    public ICommand ForgotPasswordCommand { get; set; }
    public ICommand PasswordChangedCommand { get; set; }
    public ICommand PasswordConfirmChangedCommand { get; set; }

    private readonly LoginNavigationStore _navigationStore;

    #endregion

    public LoginNewAccountModelView(LoginNavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        ForgotPasswordCommand = new ForgotPasswordCommand(navigationStore);
        PasswordChangedCommand = new RelayCommand<PasswordBox>((p)  => {return true;}, (p) => { Password = p.Password; });
        PasswordConfirmChangedCommand = new RelayCommand<PasswordBox>((p)  => {return true;}, (p) => { PasswordConfirm = p.Password; });
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
        var loginUser = DBManager.GetFirst<Model.User>(u => u.Username == UserName);
        if (loginUser == null) {
            IsIncorrect = true;
            return false;
        }
        return loginUser.VerifyPassword(Password);
    }
}