using PersonalFinanceApp.Database;
using PersonalFinanceApp.Src.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            LoginCommand = new RelayCommand<UserControl>((p) => { return true; }, (p) => { Login(p); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
        }
        void Login(UserControl p) {
            if (p == null)
                return;

            
            if(BLogin(UserName, Password)) {
                IsLogin = true;
                FrameworkElement wd = getParent(p);
                var w = wd as Window;
                if (w != null)
                {
                    w.Close();
                }
            }
            else {
                IsLogin = false;
                IsCorrect = true;
            }
           
        }
        private bool BLogin(string Username, string Password) {
            var loginUser = DBManager.GetFirst<Model.User>(u => u.Username == UserName);
            if (loginUser == null)
                return false;
            return loginUser.VerifyPassword(Password);
        }
        FrameworkElement getParent(UserControl p) {
            FrameworkElement parent = p;
            while (parent.Parent != null){
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}
