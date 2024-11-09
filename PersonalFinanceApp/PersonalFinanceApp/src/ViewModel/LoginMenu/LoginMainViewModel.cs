using PersonalFinanceApp.Database;
using PersonalFinanceApp.Src.ViewModel;
using System.Data.SQLite;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XSystem.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class LoginMainViewModel : BaseViewModel
{
    #region Properties
    
    public bool IsLogin { get; set; }
    private string _UserName;
    public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
    private string _Password;
    public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

    public ICommand LoginCommand { get; set; }
    public ICommand PasswordChangedCommand { get; set; }
    #endregion
    public LoginMainViewModel() {
        IsLogin = false;
        Password = "";
        UserName = "";
        LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
        PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
    }
    void Login(Window p) {
        if (p == null)
            return;
        string connectionString = "Data Source=.\\PFA.db;";


        using (SQLiteConnection connection = new SQLiteConnection(connectionString)) {
            connection.Open();

            string query = "SELECT COUNT(*) FROM User WHERE UserName = @username AND Password = @password"; //User and password

            using (SQLiteCommand command = new SQLiteCommand(query, connection)) {

                command.Parameters.AddWithValue("@username", UserName);
                command.Parameters.AddWithValue("@password", Password);

                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count > 0) {
                    IsLogin = true;

                    p.Close();
                }
                else {
                    IsLogin = false;
                }
            }
        }
    }

}