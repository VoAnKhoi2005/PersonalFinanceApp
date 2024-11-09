using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XAct.Users;
using XSystem.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PersonalFinanceApp.ViewModel.LoginMenu {
    public class LoginMainViewModel : BaseViewModel
    {
        #region Properties
    
        public bool IsLogin { get; set; }
        public bool IsCorrect { get; set; }
        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        #endregion
        public LoginMainViewModel() {
            IsLogin = false;
            IsCorrect = false;
            Password = "";
            UserName = "";
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { Login(p); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
        }
        void Login(Window p) {
            if (p == null)
                return;

            
            if(BLogin(UserName, Password)) {
                IsLogin = true;
                p.Close();
            }
            else {
                IsLogin = false;
                IsCorrect = true;
            }
           
        }
        private bool BLogin(string Username, string Password) {
            Model.User loginUser = DBManager.GetFirst<Model.User>(u => u.Username == Username);
            if (loginUser == null)
                return false;
            
            return loginUser.VerifyPassword(Password);
        }

    }

}
