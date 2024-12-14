using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.View;
using PersonalFinanceApp.ViewModel;
using PersonalFinanceApp.ViewModel.MainMenu;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.etc;

public interface IWindowFactory
{
    MainWindow CreateMainWindow(User user);
    MainWindow Relogin(User user);
}

public class MainWindowFactory : IWindowFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly SharedService _sharedService;
    public BaseViewModel DataContext { get; set; }

    public MainWindowFactory(BaseViewModel dataContext, IServiceProvider serviceProvider)
    {

        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _sharedService = serviceProvider.GetRequiredService<SharedService>();
        DataContext = dataContext;
        _serviceProvider = serviceProvider;
    }

    public MainWindow CreateMainWindow(User user)
    {
        _accountStore.UsersID = user.UserID.ToString();
        _accountStore.Users = user;
        NavigationStore navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
        navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<DashboardViewModel>();
        return new MainWindow(DataContext, user);
    }
    public MainWindow Relogin(User user) {
        _accountStore.UsersID = user.UserID.ToString();
        _accountStore.Users = user;
        NavigationStore navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
        navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<DashboardViewModel>();
        _sharedService.m.DataContext = DataContext;
        return _sharedService.m;
    }
}