using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class ExpenseRecycleViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseStore _expenseStore;
    private readonly AccountStore _accountStore;

    //source expense advance
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
    //choose expense advance
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

    public ICommand ExitRecoverCommand { get; set; }
    public ICommand RefreshExpenseCommand { get; set; }
    public ICommand RecoverExpenseCommand { get; set; }
    public ICommand RemoveExpenseCommand { get; set; }
    public ExpenseRecycleViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        RefreshRecycle();

        RecoverExpenseCommand = new NavigateModalCommand<ExpenseRecoverViewModel>(serviceProvider);
        RemoveExpenseCommand = new NavigateModalCommand<ExpenseRemoveViewModel>(serviceProvider);

        ExitRecoverCommand = new RelayCommand<object>(CloseModal);
        RefreshExpenseCommand = new RelayCommand<object>(RefreshRecycle);
    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void RefreshRecycle(object? sender = null) {
        Expensesadvances.Clear();
        if (_expenseStore.ExpenseBook == null) return;
        var exB = _expenseStore.ExpenseBook;
        var exp = DBManager.GetCondition<Expense>(e => e.UserID == int.Parse(_accountStore.UsersID) && exB.Year == e.ExBYear && exB.Month == e.ExBMonth, getDeleted: true);
        foreach (var e in exp) {
            Expensesadvances.Add(new ExpenseAdvance(e));
        }
    }
    public class ExpenseAdvance {
        public Expense exp { get; set; }
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
        public ExpenseAdvance(Expense ex) {
            exp = ex;
            ExpenseID = ex.ExpenseID;
            Amount = ex.Amount;
            Name = ex.Name;
            Description = ex.Description;
            Date = ex.Date;
            TimeAdded = ex.TimeAdded;
            CategoryID = ex.CategoryID;
            var cate = DBManager.GetFirst<Category>(c => c.CategoryID == ex.CategoryID && c.UserID == ex.UserID);
            Category = cate.Name;
        }
    }
    public ExpensesBook exB() {
        return _expenseStore.ExpenseBook;
    }
}
