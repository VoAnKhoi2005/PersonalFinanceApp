using System;
using System.Windows;
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
}

public class MainWindowFactory : IWindowFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    public BaseViewModel DataContext { get; set; }

    public MainWindowFactory(BaseViewModel dataContext, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        DataContext = dataContext;
        _serviceProvider = serviceProvider;
    }

    public MainWindow CreateMainWindow(User user)
    {
        _accountStore.SharedUser.Add(user.UserID.ToString());
        NavigationStore navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
        navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<DashboardViewModel>();
        return new MainWindow(DataContext, user);
    }
}