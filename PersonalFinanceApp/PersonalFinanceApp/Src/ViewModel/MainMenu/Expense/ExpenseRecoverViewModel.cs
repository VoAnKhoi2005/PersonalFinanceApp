using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class ExpenseRecoverViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseBookStore _expenseBookStore;
    private readonly ExpenseStore _expenseStore;
    public object ItemsExB {
        get => _itemsExB;
        set {
            if (_itemsExB != value) {
                _itemsExB = value;
                _expenseStore.Expenses = (Expense)value;
                OnPropertyChanged();
            }
        }
    }
    private object _itemsExB;
    public ObservableCollection<Expense> ExpensesRecover { 
        get => _expensesRecover;
        set {
            if(_expensesRecover != value) {
                _expensesRecover = value;
                OnPropertyChanged();
            }
        } 
    }
    private ObservableCollection<Expense> _expensesRecover;
    public ICommand RefreshRecoverExpenseCommand { get; set; }
    public ICommand RecoverDeleteExpenseCommand { get; set; }
    public ICommand PermanentlyDeleteExpenseCommand { get; set; }
    public ExpenseRecoverViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _expenseBookStore = serviceProvider.GetRequiredService<ExpenseBookStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        RecoverDeleteExpenseCommand = new RelayCommand<object>(RecoverExpense);
        RefreshRecoverExpenseCommand = new RelayCommand<object>(LoadExpenses);
        PermanentlyDeleteExpenseCommand = new RelayCommand<object>(DeleteExpense);
    }
    public void DeleteExpense(object parameter) {
        var itemExp = DBManager.GetFirst<Expense>(e => e.UserID == _expenseStore.Expenses.UserID && e.ExpenseID == _expenseStore.Expenses.ExpenseID);
        if (itemExp != null) {
            DBManager.Remove<Expense>(itemExp);
        }
    }
    public void RecoverExpense(object parameter) {
        var itemExp = DBManager.GetFirst<Expense>(e => e.UserID == _expenseStore.Expenses.UserID && e.ExpenseID == _expenseStore.Expenses.ExpenseID);
        if (itemExp != null) {
            itemExp.Deleted = false;
        }
        DBManager.Update<Expense>(itemExp);
    }
    public void LoadExpenses(object p) {
        //load data to datagrid
        var items = DBManager.GetCondition<Expense>(exp => exp.Deleted == true && _expenseBookStore.ExpenseBook.UserID == exp.UserID && _expenseBookStore.ExpenseBook.Year == exp.ExBYear && _expenseBookStore.ExpenseBook.Month == exp.ExBMonth);
        ExpensesRecover = new(items);
    }
}