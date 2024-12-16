using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.etc;
using PersonalFinanceApp.View;
using PersonalFinanceApp.ViewModel.LoginMenu;
using PersonalFinanceApp.ViewModel.MainMenu;
using PersonalFinanceApp.ViewModel.Stores;
using PersonalFinanceApp.Src.ViewModel.Stores;

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
            services.AddSingleton<ModalNavigationStore>();
            services.AddSingleton<SharedDataService>();
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<SharedService>();
            services.AddSingleton<AccountStore>();
            services.AddSingleton<ExpenseStore>();
            services.AddSingleton<EmailService>();
            services.AddSingleton<GoalStore>();

            //Login window
            services.AddSingleton<LoginMainViewModel>(s => new LoginMainViewModel(s));
            services.AddSingleton<LoginWindow>(s => new LoginWindow
            {
                DataContext = s.GetRequiredService<LoginMainViewModel>()
            });
            services.AddTransient<ResetPasswordViewModel>(s => new ResetPasswordViewModel(s));
            services.AddTransient<LoginNewAccountViewModel>(s => new LoginNewAccountViewModel(s));
            services.AddTransient<CodeVerificationViewModel>(s => new CodeVerificationViewModel(s));
            services.AddTransient<CreateNewPasswordViewModel>(s => new CreateNewPasswordViewModel(s));

            //Main window
            services.AddSingleton<MainViewModel>(s => new MainViewModel(s));
            services.AddSingleton<IWindowFactory>(s =>
            {
                var dataContext = s.GetRequiredService<MainViewModel>();
                return new MainWindowFactory(dataContext, s);
            });
            services.AddTransient<ExpenseViewModel>(s => new ExpenseViewModel(s));
            services.AddTransient<GoalplanViewModel>(s => new GoalplanViewModel(s));
            services.AddTransient<DashboardViewModel>(s => new DashboardViewModel(s));
            services.AddTransient<SettingViewModel>(s => new SettingViewModel(s));
            services.AddTransient<RecurringViewModel>(s => new RecurringViewModel(s));

            //Modal-Popup
            //expense
            services.AddTransient<ExpenseEditViewModel>(s => new ExpenseEditViewModel(s));
            services.AddTransient<ExpenseAddNewViewModel>(s => new ExpenseAddNewViewModel(s));
            services.AddTransient<ExpenseDeleteViewModel>(s => new ExpenseDeleteViewModel(s));
            services.AddTransient<ExpenseRemoveViewModel>(s => new ExpenseRemoveViewModel(s));
            services.AddTransient<ExpenseNewExBViewModel>(s => new ExpenseNewExBViewModel(s));
            services.AddTransient<ExpenseRecoverViewModel>(s => new ExpenseRecoverViewModel(s));
            services.AddTransient<ExpenseNewCategoryViewModel>(s => new ExpenseNewCategoryViewModel(s));
            services.AddTransient<ExpenseRecycleViewModel>(s => new ExpenseRecycleViewModel(s));

            //goal
            services.AddTransient<GoalEditViewModel>(s => new GoalEditViewModel(s));
            services.AddTransient<GoalDeleteViewModel>(s => new GoalDeleteViewModel(s));
            services.AddTransient<GoalHistoryViewModel>(s => new GoalHistoryViewModel(s));
            services.AddTransient<GoalplanAddNewViewModel>(s => new GoalplanAddNewViewModel(s));
            services.AddTransient<GoalAddSavedAmountViewModel>(s => new GoalAddSavedAmountViewModel(s));
            services.AddTransient<GoalAddNewCategoryViewModel>(s => new GoalAddNewCategoryViewModel(s));

            //recurring
            services.AddTransient<RecurringAddnew>(s => new RecurringAddnew(s));
            services.AddTransient<RecurringViewModel>(s => new RecurringViewModel(s));

        }

    }
}