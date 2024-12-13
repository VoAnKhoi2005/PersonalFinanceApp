using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;
using System.Windows;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class ExpenseEditBudgetViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly ExpenseStore _expenseStore;
    private readonly SharedService _sharedService;
    #region Properties
    //month
    public string MonthExpenseBook {
        get => _monthExpenseBook;
        set {
            if (_monthExpenseBook != value) {
                _monthExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _monthExpenseBook;
    //year
    public string YearExpenseBook {
        get => _yearExpenseBook;
        set {
            if (_yearExpenseBook != value) {
                _yearExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _yearExpenseBook;
    //budget
    public string BudgetExpenseBook {
        get => _budgetExpenseBook;
        set {
            if (_budgetExpenseBook != value) {
                _budgetExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _budgetExpenseBook;

    #endregion
    public ICommand CancelExpenseBookCommand { get; set; }
    public ICommand ConfirmExpenseBookCommand { get; set; }

    public ExpenseEditBudgetViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _sharedService = serviceProvider.GetRequiredService<SharedService>();
        LoadItem();
        CancelExpenseBookCommand = new RelayCommand<object>(CloseModal);
        ConfirmExpenseBookCommand = new RelayCommand<object>(ConfirmExpenseBook);
    }
    public void LoadItem() {
        var item = _expenseStore.ExpenseBook;
        if (item != null) {
            MonthExpenseBook = item.Month.ToString();
            YearExpenseBook = item.Year.ToString();
            BudgetExpenseBook = item.Budget.ToString();
        }
    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmExpenseBook(object sender) {
        //add data to database
        long result;
        if (!long.TryParse(BudgetExpenseBook, out result)) {
            MessageBox.Show("Budget quá to! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        var expenseBook = DBManager.GetFirst<ExpensesBook>(e => e.UserID == int.Parse(_accountStore.UsersID) && _expenseStore.ExpenseBook.Month == e.Month && _expenseStore.ExpenseBook.Year == e.Year);
        expenseBook.Budget = long.Parse(BudgetExpenseBook);

        DBManager.Update(expenseBook);

        _expenseStore.TextChangedExp = expenseBook.Month.ToString() + "/" + expenseBook.Year.ToString();

        _expenseStore.ExpenseBook = expenseBook;
        _sharedService.Notify();

        _modalNavigationStore.Close();
    }
    public class Month {
        public int Number { get; set; }
        public string Name { get; set; }
        public Month(int number, string name) {
            Number = number;
            Name = name;
        }
    }
}
