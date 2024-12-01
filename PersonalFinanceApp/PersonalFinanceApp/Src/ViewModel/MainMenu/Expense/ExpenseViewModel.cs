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
    private readonly AccountStore _accountStore;
    private readonly SharedService _sharedService;
    public bool IsButtonVisible {
        get => _isButtonVisible;
        set {
            _isButtonVisible = value;
            OnPropertyChanged();
        }
    }
    private bool _isButtonVisible;
    public bool IsButtonRecover {
        get => _isButtonRecover;
        set {
            _isButtonRecover = value;
            OnPropertyChanged();
        }
    }
    private bool _isButtonRecover;
    public ExpenseAdvance ItemsExB {
        get => _itemsExB;
        set {
            if (_itemsExB != value && value != null) {
                _itemsExB = value;
                _expenseStore.Expenses = value.exp;
                OnPropertyChanged();
            }
        }
    }
    private ExpenseAdvance _itemsExB;
    public ObservableCollection<ExpenseAdvance> Expensesadvances { 
        get => _expensesAdvances;
        set {
            if (_expensesAdvances != value) {
                _expensesAdvances = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<ExpenseAdvance> _expensesAdvances = new();
    public ICommand AddNewExpenseCommand { get; set; }
    public ICommand RefreshExpenseCommand { get; set; }
    public ICommand EditExpenseCommand { get; set; }
    public ICommand DeleteExpenseCommand { get; set; }
    public ICommand RecoverExpenseCommand { get; set; }
    public ICommand RemoveExpenseCommand { get; set; }
    public ICommand FilterExpenseCommand { get; set; }
    public ICommand LoadCommand {  get; set; }
    public ICommand LoadRecoverCommand {  get; set; }

    public ExpenseViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _sharedService = serviceProvider.GetRequiredService<SharedService>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();

        AddNewExpenseCommand = new NavigateModalCommand<ExpenseAddNewViewModel>(serviceProvider);
        EditExpenseCommand = new NavigateModalCommand<ExpenseEditViewModel>(serviceProvider);
        FilterExpenseCommand = new NavigateModalCommand<ExpenseFilterViewModel>(serviceProvider);
        DeleteExpenseCommand = new NavigateModalCommand<ExpenseDeleteViewModel>(serviceProvider);
        RemoveExpenseCommand = new NavigateModalCommand<ExpenseRemoveViewModel>(serviceProvider);
        RecoverExpenseCommand = new NavigateModalCommand<ExpenseRecoverViewModel>(serviceProvider);

        LoadExpenses();

        _sharedService.TriggerAction += OnTriggerAction;

        LoadRecoverCommand = new RelayCommand<object>(LoadRecover);
        RefreshExpenseCommand = new RelayCommand<object>(LoadExpenses);
        LoadCommand = new RelayCommand<object>(Load);
    }
    private void OnTriggerAction() {
        Expensesadvances.Clear();
        var items = _expenseStore.SharedExpense;
        foreach (var item in items) {
            Expensesadvances.Add(new ExpenseAdvance(item.exp, item.BudgetExp));
        }
    }
    public void LoadRecover(object parameter) {
        IsButtonVisible = false;
        IsButtonRecover = true;
        Expensesadvances.Clear();
        var exp = DBManager.GetCondition<Expense>(e => e.UserID == int.Parse(_accountStore.UsersID), getDeleted:true);
        foreach(var e in exp) {
            var exB = DBManager.GetFirst<ExpensesBook>(ex => ex.UserID == e.UserID && ex.Month == e.ExBMonth && ex.Year == e.ExBYear);
            Expensesadvances.Add(new ExpenseAdvance(e, exB.Budget));
        }

    }
    public void Load(object parameter) {
        //filter
        Expensesadvances.Clear();
        var items = _expenseStore.SharedExpense;
        foreach(var item in items) {
            Expensesadvances.Add(new ExpenseAdvance(item.exp, item.BudgetExp));
        }
    }
    public void LoadExpenses(object? p = null) {
        IsButtonVisible = true;
        IsButtonRecover = false;
        //load data to datagrid
        Expensesadvances.Clear();
        var items = DBManager.GetCondition<Expense>(exp => exp.UserID == int.Parse(_accountStore.UsersID));
        foreach (var item in items) {
            var exB = DBManager.GetFirst<ExpensesBook>(e => e.Month == item.ExBMonth && e.Year == item.ExBYear && e.UserID == item.UserID);
            Expensesadvances.Add(new ExpenseAdvance(item, exB.Budget));
        }
    }
    public class ExpenseAdvance {
        public Expense exp {  get; set; }
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
            Recurring = ex.Recurring;
            TimeAdded = ex.TimeAdded;
            CategoryID = ex.CategoryID;
            var cate = DBManager.GetFirst<Category>(c => c.CategoryID == ex.CategoryID);
            Category = cate.Name;
        }
    }
}
