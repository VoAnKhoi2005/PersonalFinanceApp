﻿using System.Windows;

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

        
        private void showCard(FrameworkElement card)
        {
            LoginCard.Visibility = Visibility.Collapsed;
            ResetPasswordCard.Visibility = Visibility.Collapsed;
            ResetNewPasswordCard.Visibility = Visibility.Collapsed;
            CodeVerificationCard.Visibility = Visibility.Collapsed;

            card.Visibility = Visibility.Visible;
        }
    }
}