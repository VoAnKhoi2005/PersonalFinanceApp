using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class NotificationGoalCard : BaseViewModel {
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    public string NameGoal {
        get => _nameGoal;
        set {
            if (_nameGoal != value) {
                _nameGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _nameGoal;
    public string CategoryGoal {
        get => _categoryGoal;
        set {
            if (_categoryGoal != value) {
                _categoryGoal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _categoryGoal;
    public string StartEndDate {
        get => _startEndDate;
        set {
            if (_startEndDate != value) {
                _startEndDate = value;
                OnPropertyChanged();
            }
        }
    }
    private string _startEndDate;
    public NotificationGoalCard(IServiceProvider serviceProvider, object o) {
        _serviceProvider = serviceProvider;

        Load(o);
    }
    public void Load(object o) {
        if (o == null) { throw new ArgumentNullException(nameof(o)); }
        if (o is Goal goal) {
            LoadGoal(goal);
        }
        else if(o is RecurringExpense re){
            LoadRecurring(re);
        }

    }
    public void LoadGoal(Goal g) {
        NameGoal = g.Name;
        CategoryGoal = g.CategoryName;
        if (g.StartDay != null && g.Deadline != null)
            StartEndDate = DateOnly.FromDateTime((DateTime)g.StartDay).ToString("dd/MM/yyyy") + " - " + DateOnly.FromDateTime((DateTime)g.Deadline).ToString("dd/MM/yyyy");
    }
    public void LoadRecurring(RecurringExpense re) {
        NameGoal = re.Name;
        CategoryGoal = re.StartDate.ToString("dd/MM/yyyy");
        StartEndDate = re.LastTime.ToString("dd/MM/yyyy");
    }
}
