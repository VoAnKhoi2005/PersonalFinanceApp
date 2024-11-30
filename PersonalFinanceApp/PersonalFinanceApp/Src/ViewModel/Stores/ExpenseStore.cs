using PersonalFinanceApp.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PersonalFinanceApp.Src.ViewModel.Stores; 
public class ExpenseStore : INotifyPropertyChanged {
    public ObservableCollection<string> SharedExpense { get; } = new ObservableCollection<string>();

    private Expense? _expenses;
    public Expense? Expenses {
        get => _expenses;
        set {
            if (_expenses != value) {
                _expenses = value;
                OnPropertyChanged(nameof(_expenses));
            }
        }
    }
    private bool _isFilter = false;
    public bool IsFilter {
        get => _isFilter;
        set {
            if (_isFilter != value) {
                _isFilter = value;
                OnPropertyChanged(nameof(_isFilter));
            }
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
