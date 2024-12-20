using PersonalFinanceApp.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

namespace PersonalFinanceApp.Src.ViewModel.Stores;
public class RecurringStore : INotifyPropertyChanged {
    public ObservableCollection<RecurringExpense> ShareRecurring { get; set; } = new ObservableCollection<RecurringExpense>();
    public ObservableCollection<Expense> ShareExpense { get; set; } = new ObservableCollection<Expense>();

    private RecurringExpense? _recurringExpense;
    public RecurringExpense? RecurringExpense {
        get => _recurringExpense;
        set {
            if (_recurringExpense != value) {
                _recurringExpense = value;
                OnPropertyChanged(nameof(_recurringExpense));
            }
        }
    }
    public Calendar? calendar {
        get => _calendar;
        set {
            if (_calendar != value) {
                _calendar = value;
                OnPropertyChanged(nameof(_calendar));
            }
        }
    }
    private Calendar? _calendar;

    public event Action TriggerAction;
    public void NotifyRecurring() {
        TriggerAction?.Invoke();
    }
    public event Action TriggerEditRecurring;
    public void NotifyRecurringEdit() {
        TriggerEditRecurring?.Invoke();
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
