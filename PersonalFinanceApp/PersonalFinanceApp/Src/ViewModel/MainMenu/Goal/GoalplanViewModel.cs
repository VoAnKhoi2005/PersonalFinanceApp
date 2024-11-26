using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.MainMenu;
using PersonalFinanceApp.ViewModel.Command;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class GoalplanViewModel : BaseViewModel
{
    #region Command
    public ICommand AddNewGoalCommand { get; set; }
    public ICommand EditGoalCommand { get; set; }
    public ICommand DeleteGoalCommand { get; set; }
    public ICommand HistoryGoalCommand { get; set; }
    public ICommand AddNewAmountGoalCommand { get; set; }
    public ICommand NotifyGoalCommand { get; set; }
    public ICommand FavoritesGoalCommand { get; set; }
    public ICommand RefreshGoalCommand { get; set; }
    #endregion

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

        EditGoalCommand = new NavigateCommand<GoalEditViewModel>(serviceProvider);

        RefreshGoalCommand = new RelayCommand<object>(LoadedGoal);

        GoalplanCardViewModels = new ObservableCollection<GoalplanCardViewModel>();
        List<Goal> goals = DBManager.GetAll<Goal>();
        foreach (var goal in goals) {
            GoalplanCardViewModels.Add(new GoalplanCardViewModel(goal));
        }
    }
    private void LoadedGoal(object parameter) {
        //reload data to listview
        GoalplanCardViewModels.Clear();
        List<Goal> goals = DBManager.GetAll<Goal>();
        foreach (var goal in goals) {
            GoalplanCardViewModels.Add(new GoalplanCardViewModel(goal));
        }
    }
}