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

    #endregion
    private GoalplanCardViewModel() { 
    
    }

    public GoalplanCardViewModel(Goal goal)
    {
        if (goal == null) return;
        if (goal.Target >= goal.CurrentAmount && goal.Deadline <= DateTime.Now) StatusGoalCard = "Successful!";
    }
}