using System.Windows;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            User newUser = new User("admin", "pass", "admin@123");
            DBManager.Insert(newUser);
        }
    }
}
