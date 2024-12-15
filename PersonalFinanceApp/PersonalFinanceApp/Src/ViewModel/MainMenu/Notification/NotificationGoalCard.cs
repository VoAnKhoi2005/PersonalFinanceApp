using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public NotificationGoalCard(IServiceProvider serviceProvider, Goal g) {
        _serviceProvider = serviceProvider;

        LoadGoal(g);
    }
    public void LoadGoal(Goal g) {
        NameGoal = g.Name;
        CategoryGoal = g.CategoryName;
        StartEndDate = DateOnly.FromDateTime((DateTime)g.StartDay).ToString() + " - " + DateOnly.FromDateTime((DateTime)g.Deadline).ToString();
    }
}
