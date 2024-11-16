using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class MainViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

    public ICommand DashBoardNavigateCommand { get; set; }
    public ICommand GoalplanNavigateCommand { get; set; }
    public ICommand SummaryNavigateCommand { get; set; }

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _navigationStore = serviceProvider.GetRequiredService<NavigationStore>();
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

        DashBoardNavigateCommand = new NavigateCommand<DashboardViewModel>(serviceProvider);
        GoalplanNavigateCommand = new NavigateCommand<GoalplanViewModel>(serviceProvider);
        SummaryNavigateCommand = new NavigateCommand<SummaryViewModel>(serviceProvider);
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