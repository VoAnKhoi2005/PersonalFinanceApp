using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class GoalHistoryViewModel : BaseViewModel {

    private readonly ModalNavigationStore _modalNavigationStore;
    private ObservableCollection<GoalHistory> _goalHistories = new();
    public ObservableCollection<GoalHistory> GoalHistories {
        get => _goalHistories;
        set {
            if (_goalHistories != value) {
                _goalHistories = value;
                OnPropertyChanged();
            }
        }
    }
    public ICommand RefreshHistoryGoalCommand { get; set; }
    public ICommand BackHistoryGoalCommand { get; set; }
    public bool HasNoHistoryGoal { get; set; } = true;

    public GoalHistoryViewModel(IServiceProvider serviceProvider) {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        RefreshHistoryGoalCommand = new RelayCommand<Object>(LoadHistoryGoal);
        BackHistoryGoalCommand = new RelayCommand<object>(CloseModal);

    }
    public void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    public void LoadHistoryGoal(object p) {
        //load data to datagrid

    }
}
