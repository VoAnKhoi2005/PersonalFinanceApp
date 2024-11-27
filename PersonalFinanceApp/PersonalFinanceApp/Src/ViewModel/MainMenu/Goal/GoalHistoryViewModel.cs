using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class GoalHistoryViewModel : BaseViewModel {

    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly GoalStore _goalStore;
    #region Properties
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
    #endregion
    public ICommand BackHistoryGoalCommand { get; set; }
    public bool HasNoHistoryGoal { get; set; } = true;

    public GoalHistoryViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _goalStore = serviceProvider.GetRequiredService<GoalStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        BackHistoryGoalCommand = new RelayCommand<object>(CloseModal);
        LoadHistoryGoal();
    }
    public void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    public void LoadHistoryGoal(object? parameter = null) {
        //load data to datagrid
        var idgoal = _goalStore.GoalID;
        var item = DBManager.GetCondition<GoalHistory>(g => g.GoalID == int.Parse(idgoal));
        GoalHistories = new(item);
    }
}
