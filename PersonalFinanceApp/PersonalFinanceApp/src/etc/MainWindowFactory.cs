using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Model;
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
    public BaseViewModel DataContext { get; set; }

    public MainWindowFactory(BaseViewModel dataContext, IServiceProvider serviceProvider)
    {
        DataContext = dataContext;
        _serviceProvider = serviceProvider;
    }

    public MainWindow CreateMainWindow(User user)
    {
        NavigationStore navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
        navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<DashboardViewModel>();
        return new MainWindow(DataContext, user);
    }
}