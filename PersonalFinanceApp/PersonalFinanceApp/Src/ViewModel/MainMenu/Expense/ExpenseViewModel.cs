using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class ExpenseViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
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
    public ObservableCollection<Expense> Expenses { 
        get => _expenses;
        set {
            if (_expenses != value) {
                _expenses = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<Expense> _expenses;
    public ICommand AddNewExpenseCommand { get; set; }
    public ICommand EditExpenseCommand { get; set; }
    public ICommand DeleteExpenseCommand { get; set; }
    public ICommand RefreshExpenseCommand { get; set; }
    public ICommand RecoverExpenseCommand { get; set; }

    public bool HasNoExpense { get; set; } = true;
    public ExpenseViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();

        AddNewExpenseCommand = new NavigateModalCommand<ExpenseAddNewViewModel>(serviceProvider);
        EditExpenseCommand = new NavigateModalCommand<ExpenseEditViewModel>(serviceProvider);

        RefreshExpenseCommand = new RelayCommand<object>(LoadExpenses);
    }
    public void LoadExpenses(object p) {
        //load data to datagrid
        var items = DBManager.GetCondition<Expense>(exp => exp.Deleted == false);
        Expenses = new(items);
    }
}
