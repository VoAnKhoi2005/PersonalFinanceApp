using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class GoalplanCardViewModel:BaseViewModel
{
    #region Properties
    private string _statusGoalCard = "Not finished yet";
    public string StatusGoalCard {
        get => _statusGoalCard;
        set {
            if (_statusGoalCard != value) {
                _statusGoalCard = value;
                OnPropertyChanged();
            }
        }
    }
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
    #endregion
    private GoalplanCardViewModel() { 
    
    }

    public GoalplanCardViewModel(Goal goal)
    {
        if (goal == null) return;
        if (goal.Target >= goal.CurrentAmount && goal.Deadline <= DateTime.Now) StatusGoalCard = "Successful!";
    }
}