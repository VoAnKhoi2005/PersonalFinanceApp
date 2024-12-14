using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class LoginMainViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly SharedService _SharedService;
    public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;
    public ICommand CloseCommand { get; set; }
    public ICommand WindowMinimum { get; set; }
    public ICommand WindowMaximum { get; set; }
    public ICommand MoveCommand { get; set; }
    public LoginMainViewModel(IServiceProvider serviceProvider)
    {
        _SharedService = serviceProvider.GetRequiredService<SharedService>();
        _navigationStore = serviceProvider.GetRequiredService<NavigationStore>();
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        CloseCommand = new RelayCommand<Window>(CloseWindow);
        WindowMaximum = new RelayCommand<Window>(w =>
                    w.WindowState = w.WindowState == WindowState.Maximized
                                    ? WindowState.Normal
                                    : WindowState.Maximized);
        WindowMinimum = new RelayCommand<Window>(w => w.WindowState = WindowState.Minimized);
        MoveCommand = new RelayCommand<Window>(w => w?.DragMove());
    }
    public void CloseWindow(Window? window) {
        window?.Close();
        if (_SharedService.m != null) {
            _SharedService.m.Close();
        }
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