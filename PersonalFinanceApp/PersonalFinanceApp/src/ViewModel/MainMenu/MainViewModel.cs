using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using Windows.UI;
using XAct;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class MainViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly ModalNavigationStore _modalNavigationStore;
    public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;
    public BaseViewModel? CurrentModalViewModel => _modalNavigationStore.CurrentModalViewModel;
    public bool IsModalOpen => _modalNavigationStore.IsOpen;

    public ICommand DashBoardNavigateCommand { get; set; }
    public ICommand ExpenseBookNavigateCommand { get; set; }
    public ICommand GoalplanNavigateCommand { get; set; }
    public ICommand SummaryNavigateCommand { get; set; }
    public ICommand CloseCommand { get; set; }
    public ICommand WindowMinimum { get; set; }
    public ICommand WindowMaximum { get; set; }

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _navigationStore = serviceProvider.GetRequiredService<NavigationStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        _modalNavigationStore.CurrentModalViewModelChanged += OnCurrentModalViewModelChanged;
        
        DashBoardNavigateCommand = new NavigateCommand<DashboardViewModel>(serviceProvider);
        ExpenseBookNavigateCommand = new NavigateCommand<ExpenseViewModel>(serviceProvider);
        GoalplanNavigateCommand = new NavigateCommand<GoalplanViewModel>(serviceProvider);
        SummaryNavigateCommand = new NavigateCommand<SummaryViewModel>(serviceProvider);
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
    private void OnCurrentModalViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentModalViewModel));
        //CurrentModalViewModel.StartUp();
        OnPropertyChanged(nameof(IsModalOpen));
        
    }
    public override void StartUp() {
        base.StartUp();
    }
    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    public override void Dispose()
    {
        base.Dispose();
        _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
        _modalNavigationStore.CurrentModalViewModelChanged -= OnCurrentModalViewModelChanged;
    }
}