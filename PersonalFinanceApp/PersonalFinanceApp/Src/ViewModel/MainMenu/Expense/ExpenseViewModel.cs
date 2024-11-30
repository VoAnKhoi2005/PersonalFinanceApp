using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.Model;
using System.Windows;


namespace PersonalFinanceApp.ViewModel.MainMenu;
public class ExpenseViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseStore _expenseStore;
    private readonly AccountStore _accountStore;
    public bool IsButtonVisible {
        get => _isButtonVisible;
        set {
            _isButtonVisible = value;
            OnPropertyChanged();
        }
    }
    private bool _isButtonVisible;
    public bool Filter {
        get => _filter;
        set {
            if (_filter != value) {
                _filter = value;
                CheckFilterCommand.Execute(this);
                OnPropertyChanged();
            }
        }
    }
    private bool _filter;
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
    public ICommand EditExpenseCommand { get; set; }
    public ICommand DeleteExpenseCommand { get; set; }
    public ICommand RefreshExpenseCommand { get; set; }
    public ICommand RecoverExpenseCommand { get; set; }
    public ICommand FilterExpenseCommand { get; set; }
    public ICommand CheckFilterCommand {  get; set; }
    public ICommand LoadCommand {  get; set; }
    public ICommand LoadRecoverCommand {  get; set; }

    public bool HasNoExpense { get; set; } = true;
    public ExpenseViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();

        AddNewExpenseCommand = new NavigateModalCommand<ExpenseAddNewViewModel>(serviceProvider);
        EditExpenseCommand = new NavigateModalCommand<ExpenseEditViewModel>(serviceProvider);
        FilterExpenseCommand = new NavigateModalCommand<ExpenseRecoverViewModel>(serviceProvider);
        DeleteExpenseCommand = new NavigateModalCommand<ExpenseDeleteViewModel>(serviceProvider);

        LoadExpenses();
        LoadRecoverCommand = new RelayCommand<object>(LoadRecover);
        RecoverExpenseCommand = new RelayCommand<object>(RecoverExpense);
        RefreshExpenseCommand = new RelayCommand<object>(LoadExpenses);
        CheckFilterCommand = new RelayCommand<object>(Check);
        LoadCommand = new RelayCommand<object>(Load);
    }
    public void LoadRecover(object parameter) {
        IsButtonVisible = true;
        Expensesadvances.Clear();
        var exp = DBManager.GetCondition<Expense>(e => e.UserID == int.Parse(_accountStore.UsersID), getDeleted:true);
        foreach(var e in exp) {
            var exB = DBManager.GetFirst<ExpensesBook>(ex => ex.UserID == e.UserID && ex.Month == e.ExBMonth && ex.Year == e.ExBYear);
            Expensesadvances.Add(new ExpenseAdvance(e, exB.Budget));
        }

    }
    public void Load(object parameter) {
        //if (parameter != null)
        //    MessageBox.Show("Load");
        //if (_expenseStore.IsFilter != Filter) {
            //Filter = _expenseStore.IsFilter;
        //}
    }
    //public void DeleteExpense(object parameter) {
    //    var item = _expenseStore.Expenses;
    //    var exp = DBManager.GetFirst<Expense>(e => e.UserID == int.Parse(_accountStore.UsersID) && e.ExpenseID == item.ExpenseID);
    //    exp.Deleted = true;
    //    DBManager.Update<Expense>(exp);
    //}
    public void Check(object parameter) {
        if(_expenseStore.IsFilter != Filter) {
            Filter = _expenseStore.IsFilter;
        }
    }
    public void RecoverExpense(object parameter) {
        var item = _expenseStore.Expenses;
        var exp = DBManager.GetFirst<Expense>(e => e.UserID == int.Parse(_accountStore.UsersID) && e.ExpenseID == item.ExpenseID);
        exp.Deleted = false;
        DBManager.Update<Expense>(exp);
    }
    public void LoadExpenses(object? p = null) {
        IsButtonVisible = false;
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
