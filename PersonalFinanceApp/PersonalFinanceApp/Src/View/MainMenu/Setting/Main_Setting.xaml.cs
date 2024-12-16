using PersonalFinanceApp.Src.ViewModel;
using System.Windows.Controls;

namespace PersonalFinanceApp.Src.View
{
    /// <summary>
    /// Interaction logic for Main_Setting.xaml
    /// </summary>
    public partial class Main_Setting : UserControl
    {
        public Main_Setting()
        {
            InitializeComponent();
        }

        private void RadioButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Apptheme.ChangeTheme(new Uri("Resources/Dark.xaml",UriKind.Relative));
        }

        private void RadioButton_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            Apptheme.ChangeTheme(new Uri("Resources/Light.xaml", UriKind.Relative));
        }
    }
}
