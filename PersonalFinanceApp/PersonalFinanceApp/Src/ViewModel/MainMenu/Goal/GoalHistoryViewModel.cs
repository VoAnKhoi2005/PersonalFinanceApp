using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.MainMenu;
using PersonalFinanceApp.ViewModel.Stores;
using PersonalFinanceApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PersonalFinanceApp.Src.ViewModel.MainMenu;

class GoalHistoryViewModel : BaseViewModel {
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
    public bool HasNoHistoryGoal { get; set; } = true;

    public GoalHistoryViewModel(IServiceProvider serviceProvider) {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        RefreshHistoryGoalCommand = new RelayCommand<Object>(LoadHistoryGoal);
    }
    public void LoadHistoryGoal(object p) {
        //load data to datagrid

    }
}
