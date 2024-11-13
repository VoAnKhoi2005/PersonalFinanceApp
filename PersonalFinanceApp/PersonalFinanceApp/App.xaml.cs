using System.Windows;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.View;
using PersonalFinanceApp.ViewModel.LoginMenu;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new LoginNewAccountViewModel(navigationStore);

            MainWindow = new LoginWindow
            {
                DataContext = new LoginMainViewModel(navigationStore)
            };
            MainWindow.Show();

            //Create database
            using (var context = new AppDbContext())
            {
                context.EnsureDatabaseCreated();
            }
            DBManager.AutoDelete();
        }
    }

}
