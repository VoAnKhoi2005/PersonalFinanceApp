using System.Windows;
using System.Windows.Input;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class LoginNewAccountModelView : BaseViewModel
{
    #region Properties

    public bool IsLogin { get; set; }
    public bool IsCorrect { get; set; }
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

    #endregion

    public LoginNewAccountModelView(LoginNavigationStore navigationStore)
    {
        IsLogin = false;
        IsCorrect = false;
        Password = "";
        UserName = "";
        ForgotPasswordCommand = new ForgotPasswordCommand(navigationStore);
    }

    void Login(Window p)
    {
        if (p == null)
            return;


        if (BLogin(UserName, Password))
        {
            IsLogin = true;
            p.Close();
        }
        else
        {
            IsLogin = false;
            IsCorrect = true;
        }
    }

    private bool BLogin(string Username, string Password)
    {
        var loginUser = DBManager.GetFirst<Model.User>(u => u.Username == UserName);
        if (loginUser == null)
            return false;

        return loginUser.VerifyPassword(Password);
    }
}