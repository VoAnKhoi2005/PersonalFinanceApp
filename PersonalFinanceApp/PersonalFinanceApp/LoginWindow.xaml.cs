using System.Windows;

namespace PersonalFinanceApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CardResetPassword.Visibility = Visibility.Visible;
            ResetPasswordGrid.Visibility = Visibility.Visible;

        }
    }
}
