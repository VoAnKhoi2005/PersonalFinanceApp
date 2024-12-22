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

    public MainWindowFactory(IServiceProvider serviceProvider)
    {

        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _sharedService = serviceProvider.GetRequiredService<SharedService>();
        _serviceProvider = serviceProvider;
    }

    public MainWindow CreateMainWindow(User user)
    {
        _accountStore.UsersID = user.UserID.ToString();
        _accountStore.Users = user;
        NavigationStore navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
        navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<DashboardViewModel>();
        return new MainWindow(_serviceProvider.GetRequiredService<MainViewModel>(), user);
    }
    public MainWindow Relogin(User user) {
        _accountStore.UsersID = user.UserID.ToString();
        _accountStore.Users = user;
        NavigationStore navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
        navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<DashboardViewModel>();
        _sharedService.m.DataContext = _serviceProvider.GetRequiredService<MainViewModel>();
        return _sharedService.m;
    }
}