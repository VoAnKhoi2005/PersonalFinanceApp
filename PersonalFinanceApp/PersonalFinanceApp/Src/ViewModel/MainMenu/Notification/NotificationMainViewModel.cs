using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class NotificationMainViewModel : BaseViewModel {
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly GoalStore _goalStore;

    private ObservableCollection<NotificationGoalCard> _goalplanCardViewModels = new ObservableCollection<NotificationGoalCard>();
    public ObservableCollection<NotificationGoalCard> GoalNotifyCardViewModels {
        get => _goalplanCardViewModels;
        set {
            if (_goalplanCardViewModels != value) {
                _goalplanCardViewModels.CollectionChanged -= OnGoalplanCardViewModelsChanged;
                _goalplanCardViewModels = value;
                _goalplanCardViewModels.CollectionChanged += OnGoalplanCardViewModelsChanged;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasNoGoal));
            }
        }
    }
    private void OnGoalplanCardViewModelsChanged(object? sender, NotifyCollectionChangedEventArgs e) {
        OnPropertyChanged(nameof(HasNoGoal));
    }
    public bool HasNoGoal => !GoalNotifyCardViewModels.Any();

    public NotificationMainViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _goalStore = serviceProvider.GetRequiredService<GoalStore>();
        GoalNotifyCardViewModels = new ObservableCollection<NotificationGoalCard>();
        var items = DBManager.GetAll<Goal>();
        foreach(var goal in items) {
            GoalNotifyCardViewModels.Add(new NotificationGoalCard(serviceProvider, goal));
        }

    }
}
