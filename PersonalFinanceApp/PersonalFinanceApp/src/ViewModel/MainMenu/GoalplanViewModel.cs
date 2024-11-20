using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class GoalplanViewModel : BaseViewModel
{
    public ICommand AddNewGoalCommand { get; set; }

    private ObservableCollection<GoalplanCardViewModel> _goalplanCardViewModels = new ObservableCollection<GoalplanCardViewModel>();
    public ObservableCollection<GoalplanCardViewModel> GoalplanCardViewModels
    {
        get => _goalplanCardViewModels;
        set
        {
            if (_goalplanCardViewModels != value)
            {
                _goalplanCardViewModels.CollectionChanged -= OnGoalplanCardViewModelsChanged;
                _goalplanCardViewModels = value;
                _goalplanCardViewModels.CollectionChanged += OnGoalplanCardViewModelsChanged;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasNoGoal));
            }
        }
    }
    private void OnGoalplanCardViewModelsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(HasNoGoal));
    }

    public bool HasNoGoal => !GoalplanCardViewModels.Any();


    public GoalplanViewModel(IServiceProvider serviceProvider)
    {
        AddNewGoalCommand = new NavigateModalCommand<GoalplanAddNewViewModel>(serviceProvider);

        GoalplanCardViewModels = new ObservableCollection<GoalplanCardViewModel>();
        List<Goal> goals = DBManager.GetAll<Goal>();
        foreach (var goal in goals)
        {
            GoalplanCardViewModels.Add(new GoalplanCardViewModel(goal));
        }
    }
}