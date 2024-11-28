using PersonalFinanceApp.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PersonalFinanceApp.Src.ViewModel.Stores; 
public class ExpenseBookStore : INotifyPropertyChanged {
    public ObservableCollection<string> SharedExpenseBook { get; } = new ObservableCollection<string>();

    private ExpensesBook? _expenseBook;
    public ExpensesBook? ExpenseBook {
        get => _expenseBook;
        set {
            if (_expenseBook != value) {
                _expenseBook = value;
                OnPropertyChanged(nameof(_expenseBook));
            }
        }
    }
    private string? _idExB;
    public string? IdExB {
        get => _idExB;
        set {
            if (_idExB != value) {
                _idExB = value;
                OnPropertyChanged(nameof(_idExB));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
