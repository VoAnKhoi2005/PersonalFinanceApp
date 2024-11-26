using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.Globalization;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class GoalplanCardViewModel:BaseViewModel
{
    private readonly ModalNavigationStore _modalNavigationStore;
    public ICommand EditGoalCommand { get; set; }

    #region Properties
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
    //DecriptionGoalCard
    public string DecriptionGoalCard {
        get => _decriptionGoalCard;
        set {
            if (_decriptionGoalCard != value) {
                _decriptionGoalCard = value;
                OnPropertyChanged();
            }
        }
    }
    private string _decriptionGoalCard;
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
    public string ResourceGoalCard {
        get => _resourceGoalCard;
        set {
            if(_resourceGoalCard != value) {
                _resourceGoalCard = value;
                OnPropertyChanged();
            }
        }
    }
    private string _resourceGoalCard;
    #endregion
    private GoalplanCardViewModel() { 
        
    }
    public GoalplanCardViewModel(IServiceProvider serviceProvider) {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        EditGoalCommand = new NavigateCommand<GoalEditViewModel>(serviceProvider);
    }
    public GoalplanCardViewModel(Goal goal)
    {
        NameGoalCard = goal.Name;
        TargetGoalCard = goal.Target.ToString();
        CurrentAmoutGoalCard = goal.CurrentAmount.ToString();
        ReminderGoalCard = goal.Reminder;
        DeadlineGoalCard = goal.Deadline.ToString();
        StatusGoalCard = goal.Status;
        ResourceGoalCard = goal.Resources;
        CategoryGoalCard = goal.CategoryName;

        //if (goal == null) return;
        //if (goal.Target >= goal.CurrentAmount && goal.Deadline <= DateTime.Now) StatusGoalCard = "Successful!";
    }
}