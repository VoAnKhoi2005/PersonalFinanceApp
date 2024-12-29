using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using System.Windows.Input;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class NotificationGoalCard : BaseViewModel {
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly RecurringStore _recurringStore;
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
    //public Goal goal { get; set; }
    public ICommand CloseGoalNotificationCommand { get; set; }
    public NotificationGoalCard(IServiceProvider serviceProvider, Goal g) {
        _serviceProvider = serviceProvider;
        _recurringStore = serviceProvider.GetRequiredService<RecurringStore>();
        //goal = g;
        LoadGoal(g);
        CloseGoalNotificationCommand = new RelayCommand<object>(CloseGoalNotification);
    }
    public void CloseGoalNotification(object? parameter = null) {
        _recurringStore.ShareRecurring.Remove(this);
        _recurringStore.NotifyUpload();
    }
    public void LoadGoal(Goal g) {
        NameGoal = g.Name;
        CategoryGoal = g.CategoryName;
        if (g.StartDay != null && g.Deadline != null)
            StartEndDate = DateOnly.FromDateTime((DateTime)g.StartDay).ToString("dd/MM/yyyy") + " - " + DateOnly.FromDateTime((DateTime)g.Deadline).ToString("dd/MM/yyyy");
    }
}
