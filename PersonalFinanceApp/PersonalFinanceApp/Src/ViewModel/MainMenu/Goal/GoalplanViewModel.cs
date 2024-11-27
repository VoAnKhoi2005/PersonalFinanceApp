using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class GoalplanViewModel : BaseViewModel
{
    #region Command
    public ICommand AddNewGoalCommand { get; set; }

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
    private readonly IServiceProvider _serviceProvider;
    private readonly GoalStore _goalStore;


    public GoalplanViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _goalStore = serviceProvider.GetRequiredService<GoalStore>();

        GoalplanCardViewModels = new ObservableCollection<GoalplanCardViewModel>();

        AddNewGoalCommand = new NavigateModalCommand<GoalplanAddNewViewModel>(serviceProvider);

        RefreshGoalCommand = new RelayCommand<object>(LoadedGoal);

        LoadedGoal();
    }
    private void LoadedGoal(object? parameter = null) {
        //reload data to listview
        GoalplanCardViewModels.Clear();
        List<Goal> goals = DBManager.GetAll<Goal>();
        foreach (var goal in goals) {
            GoalplanCardViewModels.Add(new GoalplanCardViewModel(_serviceProvider, goal) { ID = goal.GoalID.ToString()});
        }
    }
}