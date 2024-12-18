using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class NotificationRecurringCard : BaseViewModel {
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    public string NameRecurring {
        get => _nameRecurring;
        set {
            if (_nameRecurring != value) {
                _nameRecurring = value;
                OnPropertyChanged();
            }
        }
    }
    private string _nameRecurring;
    public string StartDate {
        get => _startDate;
        set {
            if (_startDate != value) {
                _startDate = value;
                OnPropertyChanged();
            }
        }
    }
    private string _startDate;
    public string LastTime {
        get => _lastTime;
        set {
            if (_lastTime != value) {
                _lastTime = value;
                OnPropertyChanged();
            }
        }
    }
    private string _lastTime;
    public NotificationRecurringCard(IServiceProvider serviceProvider, RecurringExpense re) {
        _serviceProvider = serviceProvider;
        LoadRecurring(re);
    }
    public void LoadRecurring(RecurringExpense re) {
        NameRecurring = re.Name;
        StartDate = re.StartDate.ToString();
        LastTime = re.LastTime.ToString();
    }
}
