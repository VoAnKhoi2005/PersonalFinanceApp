using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class NotificationMainViewModel : BaseViewModel
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;

    public bool HasNoNotify => !NotifyCardViewModels.Any();
    private ObservableCollection<object> _notifyCardViewModels = new ObservableCollection<object>();
    public ObservableCollection<object> NotifyCardViewModels
    {
        get => _notifyCardViewModels;
        set
        {
            if (_notifyCardViewModels != value)
            {
                _notifyCardViewModels.CollectionChanged -= OnNotifyViewModelsChanged;
                _notifyCardViewModels = value;
                _notifyCardViewModels.CollectionChanged += OnNotifyViewModelsChanged;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasNoNotify));
            }
        }
    }

    private void OnNotifyViewModelsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(HasNoNotify));
    }

    private void OnGoalplanCardViewModelsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(HasNoNotify));
    }

    public NotificationMainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();

        NotifyCardViewModels = new ObservableCollection<object>();
        LoadNotify();
    }

    public void LoadNotify()
    {
        NotifyCardViewModels.Clear();
        LoadNotifyGoal();
        LoadNotifyRecurring();
        NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, new Goal()));
        NotifyCardViewModels.Add(new NotificationRecurringCard(_serviceProvider, new RecurringExpense()));
    }

    public void LoadNotifyGoal()
    {
        try
        {
            var items = DBManager.GetCondition<Goal>(g => g.UserID == _accountStore.Users.UserID && g.Deadline >= DateTime.Today);
            foreach (var item in items)
            {
                DateTime dt = (DateTime)item.StartDay;
                if (item.Reminder.CompareTo("Daily") == 0)
                {
                    NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, item));
                }
                else if (item.Reminder.CompareTo("Weekly") == 0)
                {
                    if (dt.AddDays(7) == DateTime.Today)
                    {
                        NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, item));
                    }
                }
                else if (item.Reminder.CompareTo("Monthly") == 0)
                {
                    if (dt.AddMonths(1) == DateTime.Today)
                    {
                        NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, item));
                    }
                }
                else
                {
                    //yearly
                    if (dt.AddYears(1) == DateTime.Today)
                    {
                        NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, item));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void LoadNotifyRecurring()
    {
        try
        {
            var recurring = DBManager.GetCondition<RecurringExpense>(re => re.LastTime <= DateOnly.FromDateTime(DateTime.Now) && re.UserID == _accountStore.Users.UserID);
            foreach (var rec in recurring)
            {
                while (rec.LastTime <= DateOnly.FromDateTime(DateTime.Today))
                {
                    if (rec.Frequency.CompareTo("Daily") == 0)
                    {
                        rec.LastTime = rec.LastTime.AddDays(rec.Interval);
                    }
                    else if (rec.Frequency.CompareTo("Weekly") == 0)
                    {
                        rec.LastTime = rec.LastTime.AddDays(rec.Interval * 7);
                    }
                    else if (rec.Frequency.CompareTo("Monthly") == 0)
                    {
                        rec.LastTime = rec.LastTime.AddMonths(rec.Interval);
                    }
                    else if (rec.Frequency.CompareTo("Yearly") == 0)
                    {
                        rec.LastTime = rec.LastTime.AddYears(rec.Interval);
                    }

                    if (rec.LastTime <= DateOnly.FromDateTime(DateTime.Today))
                    {
                        NotifyCardViewModels.Add(new NotificationGoalCard(_serviceProvider, rec));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}