using System.Windows;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;

namespace PersonalFinanceApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            User newUser = new User("khoi", "123456", "khoi@gmail.com");
            DBManager.Insert(newUser);
        }

        
        private void showCard(FrameworkElement card)
        {
            //LoginCard.Visibility = Visibility.Collapsed;
            //ResetPasswordCard.Visibility = Visibility.Collapsed;
            //ResetNewPasswordCard.Visibility = Visibility.Collapsed;
            //CodeVerificationCard.Visibility = Visibility.Collapsed;

            //card.Visibility = Visibility.Visible;
        }
    }
}
