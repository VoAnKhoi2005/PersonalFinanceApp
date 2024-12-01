using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class LoginMainViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;
    public ICommand CloseCommand { get; set; }
    public ICommand WindowMinimum { get; set; }
    public ICommand WindowMaximum { get; set; }
    public LoginMainViewModel(IServiceProvider serviceProvider)
    {
        _navigationStore = serviceProvider.GetRequiredService<NavigationStore>();
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        CloseCommand = new RelayCommand<Window>(Close);
        WindowMaximum = new RelayCommand<Window>(max);
        WindowMinimum = new RelayCommand<Window>(mini);
    }
    public void max(Window window) {
        window.WindowState = window.WindowState == WindowState.Maximized
                    ? WindowState.Normal
                    : WindowState.Maximized;
    }
    public void mini(Window window) {
        window.WindowState = WindowState.Minimized;
    }
    public void Close(Window window) {
        window?.Close();
    }
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    public override void Dispose()
    {
        base.Dispose();
        _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
    }
}