using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.etc;
using PersonalFinanceApp.View;
using PersonalFinanceApp.ViewModel.LoginMenu;
using PersonalFinanceApp.ViewModel.MainMenu;
using PersonalFinanceApp.ViewModel.Stores;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.Src.View;

namespace PersonalFinanceApp
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //Default login window
            NavigationStore navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
            navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<LoginNewAccountViewModel>();
            MainWindow = _serviceProvider.GetRequiredService<LoginWindow>();

            //Default main window

            //NavigationStore navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
            //navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<DashboardViewModel>();
            //MainWindow = _serviceProvider.GetRequiredService<IWindowFactory>().CreateMainWindow(null);

            //MainWindow = new TestWindow();

            MainWindow.Show();

            //Create database
            using (var context = new AppDbContext())
            {
                context.EnsureDatabaseCreated();
            }

            DBManager.AutoDelete();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<ModalNavigationStore>();
            services.AddSingleton<SharedDataService>();
            services.AddSingleton<AccountStore>();
            services.AddSingleton<GoalStore>();
            services.AddSingleton<ExpenseStore>();
            services.AddSingleton<SharedService>();

            //Login window
            services.AddSingleton<LoginMainViewModel>(s => new LoginMainViewModel(s));
            services.AddSingleton<LoginWindow>(s => new LoginWindow
            {
                DataContext = s.GetRequiredService<LoginMainViewModel>()
            });
            services.AddTransient<LoginNewAccountViewModel>(s => new LoginNewAccountViewModel(s));
            services.AddTransient<ResetPasswordViewModel>(s => new ResetPasswordViewModel(s));
            services.AddTransient<CodeVerificationViewModel>(s => new CodeVerificationViewModel(s));
            services.AddTransient<CreateNewPasswordViewModel>(s => new CreateNewPasswordViewModel(s));

            //Main window
            services.AddSingleton<MainViewModel>(s => new MainViewModel(s));
            services.AddSingleton<IWindowFactory>(s =>
            {
                var dataContext = s.GetRequiredService<MainViewModel>();
                return new MainWindowFactory(dataContext, s);
            });
            services.AddTransient<DashboardViewModel>(s => new DashboardViewModel(s));
            services.AddTransient<ExpenseViewModel>(s => new ExpenseViewModel(s));
            services.AddTransient<GoalplanViewModel>(s => new GoalplanViewModel(s));
            services.AddTransient<SummaryViewModel>(s => new SummaryViewModel(s));


            //Modal-Popup
            //expense
            services.AddTransient<ExpenseAddNewViewModel>(s => new ExpenseAddNewViewModel(s));
            services.AddTransient<ExpenseEditViewModel>(s => new ExpenseEditViewModel(s));
            services.AddTransient<ExpenseDeleteViewModel>(s => new ExpenseDeleteViewModel(s));
            services.AddTransient<ExpenseRemoveViewModel>(s => new ExpenseRemoveViewModel(s));
            services.AddTransient<ExpenseRecoverViewModel>(s => new ExpenseRecoverViewModel(s));
            services.AddTransient<ExpenseFilterViewModel>(s => new ExpenseFilterViewModel(s));
            services.AddTransient<ExpenseNewExBViewModel>(s => new ExpenseNewExBViewModel(s));
            services.AddTransient<ExpenseNewCategoryViewModel>(s => new ExpenseNewCategoryViewModel(s));


            //goal
            services.AddTransient<GoalplanAddNewViewModel>(s => new GoalplanAddNewViewModel(s));
            services.AddTransient<GoalEditViewModel>(s => new GoalEditViewModel(s));
            services.AddTransient<GoalHistoryViewModel>(s => new GoalHistoryViewModel(s));
            services.AddTransient<GoalAddSavedAmountViewModel>(s => new GoalAddSavedAmountViewModel(s));
            services.AddTransient<GoalDeleteViewModel>(s => new GoalDeleteViewModel(s));
            services.AddTransient<GoalAddNewCategoryViewModel>(s => new GoalAddNewCategoryViewModel(s));

        }

    }
}