using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PersonalFinanceApp.Src.ViewModel.Stores; 
public class ExpenseStore : INotifyPropertyChanged {
    public ObservableCollection<ExpenseAdvance> SharedExpense { get; set; } = new ObservableCollection<ExpenseAdvance>();

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
    public string BudgetCurrentExb {
        get => _budgetCurrentExb;
        set {
            if (_budgetCurrentExb != value) {
                _budgetCurrentExb = value;
                OnPropertyChanged(nameof(_budgetCurrentExb));
            }
        }
    }
    private string _budgetCurrentExb;

    private Category? _categorys;
    public Category? Categorys {
        get => _categorys;
        set {
            if (_categorys != value) {
                _categorys = value;
                OnPropertyChanged(nameof(_categorys));
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
    public string TextChangedExp {
        get => _textChangedExp;
        set {
            if (_textChangedExp != value) {
                _textChangedExp = value;
                OnPropertyChanged(nameof(_textChangedExp));
            }
        }
    }
    private string _textChangedExp;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    public class ExpenseAdvance {
        public Expense exp { get; set; }
        public long BudgetExp { get; set; }
        public int ExpenseID { get; set; }
        public long Amount { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
        public bool Recurring { get; set; }
        public DateTime TimeAdded { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public ExpenseAdvance() { }
        public ExpenseAdvance(Expense ex, long budget) {
            exp = ex;
            BudgetExp = budget;
            ExpenseID = ex.ExpenseID;
            Amount = ex.Amount;
            Name = ex.Name;
            Description = ex.Description;
            Date = ex.Date;
            TimeAdded = ex.TimeAdded;
            CategoryID = ex.CategoryID;
            var cate = DBManager.GetFirst<Category>(c => c.CategoryID == ex.CategoryID);
            Category = cate.Name;
        }
    }
}
