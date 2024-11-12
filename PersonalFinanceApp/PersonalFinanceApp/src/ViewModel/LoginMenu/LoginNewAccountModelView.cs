using System.Windows;
using System.Windows.Input;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class LoginNewAccountModelView : BaseViewModel
{
    #region Properties

    public bool IsLogin { get; set; } = false;

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

    public ICommand LoginCommand { get; set; }
    public ICommand ForgotPasswordCommand { get; set; }
    private readonly LoginNavigationStore _navigationStore;

    #endregion

    public LoginNewAccountModelView(LoginNavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        ForgotPasswordCommand = new ForgotPasswordCommand(navigationStore);
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
        if (loginUser == null)
            return false;

        return loginUser.VerifyPassword(Password);
    }
}