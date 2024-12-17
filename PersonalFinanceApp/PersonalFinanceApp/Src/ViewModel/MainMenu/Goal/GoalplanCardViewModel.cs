using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;
using OxyPlot;
using OxyPlot.Series;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class GoalplanCardViewModel:BaseViewModel
{
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly GoalStore _goalStore;

    public PlotModel ProgressPlotModel { get; set; }
    public PlotController CustomPlotController { get; set; }

    #region Properties
    //id
    public string ID {
        get => _iD;
        set {
            if (_iD != value) {
                _iD = value;
                OnPropertyChanged();
            }
        }
    }
    private string _iD;
    //can add amount
    public bool CanAddAmount {
        get => _canAddAmount;
        set {
            if (_canAddAmount != value) {
                _canAddAmount = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _canAddAmount;
    //status
    public string StatusGoalCard {
        get => _statusGoalCard;
        set {
            if (_statusGoalCard != value) {
                _statusGoalCard = value;
                OnPropertyChanged();
            }
        }
    }
    private string _statusGoalCard = "Active";
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
    //target
    public string TargetGoalCard {
        get => _targetGoalCard;
        set {
            if (_targetGoalCard != value) {
                _targetGoalCard = value;
                OnPropertyChanged();
            }
        }
    }
    private string _targetGoalCard;
    //startDateGoalCard
    public string StartDateGoalCard {
        get => _startDateGoalCard;
        set {
            if (_startDateGoalCard != value) {
                _startDateGoalCard = value;
                OnPropertyChanged();
            }
        }
    }
    private string _startDateGoalCard;
    //startDateGoalCard
    public string DeadlineGoalCard {
        get => _deadlineGoalCard;
        set {
            if (_deadlineGoalCard != value) {
                _deadlineGoalCard = value;
                OnPropertyChanged();
            }
        }
    }
    private string _deadlineGoalCard;
    //CurrentAmoutGoalCard
    public string CurrentAmoutGoalCard {
        get => _currentAmoutGoalCard;
        set {
            if (_currentAmoutGoalCard != value) {
                _currentAmoutGoalCard = value;
                OnPropertyChanged();
            }
        }
    }
    private string _currentAmoutGoalCard;
    //categorygoalcard
    public string CategoryGoalCard {
        get => _categoryGoalCard;
        set {
            if(_categoryGoalCard != value) {
                _categoryGoalCard = value;
                OnPropertyChanged();
            }
        }
    }
    private string _categoryGoalCard;
    //reminderGoalCard
    public string ReminderGoalCard {
        get => _reminderGoalCard;
        set {
            if(_reminderGoalCard != value) {
                _reminderGoalCard = value;
                OnPropertyChanged();    
            }
        }
    }
    private string _reminderGoalCard;
    //resourceGoalCard
    public string DescriptionGoalCard {
        get => _descriptionGoalCard;
        set {
            if (_descriptionGoalCard != value) {
                _descriptionGoalCard = value;
                OnPropertyChanged();
            }
        }
    }
    private string _descriptionGoalCard;
    //goal
    private string _progressPercentage;
    public string ProgressPercentage
    {
        get => _progressPercentage;
        set
        {
            _progressPercentage = value;
            OnPropertyChanged();
        }
    }
    #endregion
    #region Command
    public ICommand EditGoalCommand { get; set; }
    public ICommand DeleteGoalCommand { get; set; }
    public ICommand HistoryGoalCommand { get; set; }
    public ICommand AddNewAmountGoalCommand { get; set; }
    public ICommand SaveIDGoalCard { get; set; }
    #endregion
    private GoalplanCardViewModel() { }
    public GoalplanCardViewModel(IServiceProvider serviceProvider, Goal goal)
    {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _goalStore = serviceProvider.GetRequiredService<GoalStore>();

        SaveIDGoalCard = new RelayCommand<object>(SaveID);
        EditGoalCommand = new NavigateModalCommand<GoalEditViewModel>(serviceProvider);
        DeleteGoalCommand = new NavigateModalCommand<GoalDeleteViewModel>(serviceProvider);
        AddNewAmountGoalCommand = new NavigateModalCommand<GoalAddSavedAmountViewModel>(serviceProvider);
        HistoryGoalCommand = new NavigateModalCommand<GoalHistoryViewModel>(serviceProvider);


        if (goal == null)
            return;

        CustomPlotController = new PlotController();
        CustomPlotController.UnbindMouseDown(OxyMouseButton.Left);
        CustomPlotController.BindMouseEnter(PlotCommands.HoverSnapTrack);

        ProgressPlotModel = CreateProgressPlotModel(goal);
        LoadGoalCard(goal);
    }
    public void LoadGoalCard(Goal goal) 
    {
        NameGoalCard = goal.Name;
        TargetGoalCard = goal.Target.ToString();
        CurrentAmoutGoalCard = goal.CurrentAmount.ToString();
        ReminderGoalCard = goal.Reminder;
        DeadlineGoalCard = goal.Deadline.ToString();
        StatusGoalCard = goal.Status;
        CategoryGoalCard = goal.CategoryName;
        DescriptionGoalCard = goal.Description;

        if(goal.Target <= goal.CurrentAmount || DateTime.Now > goal.Deadline) {
            CanAddAmount = false;
        }
        else {
            CanAddAmount = true;
        }
    }
    public void SaveID(object sender) {
        _goalStore.GoalID = ID;
    }

    public PlotModel CreateProgressPlotModel(Goal goal)
    {
        PlotModel plotModel = new PlotModel();
        var pieSeries = new PieSeries
        {
            InnerDiameter = 0.7,
            StrokeThickness = 0,
            OutsideLabelFormat = null,
            InsideLabelFormat = null,
            AngleSpan = 360,
            StartAngle = -90,
            TextColor = OxyColors.White,
            TrackerFormatString = "{Label}: {Value:#,0} VND ({3:P1})",
        };

        pieSeries.Slices.Add(new PieSlice("Current Amount", goal.CurrentAmount)
        {
            Fill = OxyColors.DodgerBlue
        });
        pieSeries.Slices.Add(new PieSlice("Remaining Amount", goal.Target - goal.CurrentAmount)
        {
            Fill = OxyColors.Gray
        });

        plotModel.Series.Add(pieSeries);
        double percentage = goal.CurrentAmount * 100.0 / goal.Target;
        ProgressPercentage = percentage.ToString("0.0") + "%";
        return plotModel;
    }
    public PlotModel CreateProgressPlotModelRandom()
    {
        Random random = new Random();
        PlotModel plotModel = new PlotModel();
        var pieSeries = new PieSeries
        {
            InnerDiameter = 0.7,
            StrokeThickness = 0,
            OutsideLabelFormat = null,
            InsideLabelFormat = null,
            AngleSpan = 360,
            StartAngle = -90,
            TextColor = OxyColors.White,
            TrackerFormatString = "{Label}: {Value:#,0} VND ({3:P1})",
        };
        long CurrentAmount = random.Next(1000000, 100000000);
        pieSeries.Slices.Add(new PieSlice("Current Amount", CurrentAmount)
        {
            Fill = OxyColors.DodgerBlue
        });
        pieSeries.Slices.Add(new PieSlice("Remaining Amount", 100000000 - CurrentAmount)
        {
            Fill = OxyColors.Gray
        });

        plotModel.Series.Add(pieSeries);
        double percentage = CurrentAmount * 100.0 / 100000000;
        ProgressPercentage = percentage.ToString("0.0") + "%";
        return plotModel;
    }
}