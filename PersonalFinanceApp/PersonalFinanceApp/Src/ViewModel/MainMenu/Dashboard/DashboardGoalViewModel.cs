using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class DashboardGoalViewModel : BaseViewModel {
    private readonly IServiceProvider _serviceProvider;
    //name
    public string NameGoalCard {
        get => _nameGoalCard;
        set {
            if (_nameGoalCard != value) {
                _nameGoalCard = value;
                OnPropertyChanged();
            }
        }
    }
    private string _nameGoalCard;
    //value
    public string ProcessValue {
        get => _processValue;
        set {
            if (_processValue != value) {
                _processValue = value;
                OnPropertyChanged();
            }
        }
    }
    private string _processValue;
    public string AmountTarget {
        get => _amountTarget;
        set {
            if (_amountTarget != value) {
                _amountTarget = value;
                OnPropertyChanged();
            }
        }
    }
    private string _amountTarget;
    #region Command
    public ICommand AddNewGoalCommand { get; set; }
    public ICommand RefreshGoalCommand { get; set; }
    #endregion
    public DashboardGoalViewModel(IServiceProvider serviceProvider, Goal g) {
        _serviceProvider = serviceProvider;
        AddNewGoalCommand = new NavigateModalCommand<GoalplanAddNewViewModel>(serviceProvider);
        if (g == null)
            return;
        LoadGoal(g);
    }
    public void LoadGoal(Goal g) {
        NameGoalCard = g.Name;
        ProcessValue = ((g.CurrentAmount * 100 / g.Target) == 0) ? "1": (g.CurrentAmount * 100 / g.Target).ToString();
        AmountTarget = g.CurrentAmount.ToString();

    }
}
