using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.MainMenu;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

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

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _navigationStore = serviceProvider.GetRequiredService<NavigationStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        _modalNavigationStore.CurrentModalViewModelChanged += OnCurrentModalViewModelChanged;
        
        DashBoardNavigateCommand = new NavigateCommand<DashboardViewModel>(serviceProvider);
        ExpenseBookNavigateCommand = new NavigateCommand<ExpenseBookViewModel>(serviceProvider);
        GoalplanNavigateCommand = new NavigateCommand<GoalplanViewModel>(serviceProvider);
        SummaryNavigateCommand = new NavigateCommand<SummaryViewModel>(serviceProvider);
    }

    private void OnCurrentModalViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentModalViewModel));
        OnPropertyChanged(nameof(IsModalOpen));
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