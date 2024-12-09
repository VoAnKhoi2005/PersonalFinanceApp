using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class ExpenseNewExBViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly ExpenseStore _expenseStore;
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
    //itemsource month
    public ObservableCollection<Month> Months {
        get => _months;
        set {
            if (_months != value) {
                _months = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<Month> _months;
    //itemsource year
    public ObservableCollection<int> Years {
        get => _years;
        set {
            if (_years != value) {
                _years = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<int> _years = new();
    //selected item month
    public Month SelectedMonth {
        get => _selectedMonth;
        set {
            if (_selectedMonth != value) {
                _selectedMonth = value;
                OnPropertyChanged();
            }
        }
    }
    private Month _selectedMonth;
    //selected item year
    public int SelectedYear {
        get => _selectedYear;
        set {
            if (_selectedYear != value) {
                _selectedYear = value;
                OnPropertyChanged();
            }
        }
    }
    private int _selectedYear;

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

    public ExpenseNewExBViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        LoadItem();
        CancelExpenseBookCommand = new RelayCommand<object>(CloseModal);
        ConfirmExpenseBookCommand = new RelayCommand<object>(ConfirmExpenseBook);
    }
    public void LoadItem() {
        Months = new ObservableCollection<Month>{
                new Month(1, "January"),
                new Month(2, "February"),
                new Month(3, "March"),
                new Month(4, "April"),
                new Month(5, "May"),
                new Month(6, "June"),
                new Month(7, "July"),
                new Month(8, "August"),
                new Month(9, "September"),
                new Month(10, "October"),
                new Month(11, "November"),
                new Month(12, "December"),
            };

        Years = new ObservableCollection<int>();
        for (int year = 2000; year <= DateTime.Now.Year; year++) {
            Years.Add(year);
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
        var expenseBook = new ExpensesBook() {
            Month = SelectedMonth.Number,
            Year = SelectedYear,
            Budget = long.Parse(BudgetExpenseBook),
            UserID = int.Parse(_accountStore.UsersID),
        };
        DBManager.Insert(expenseBook);
        _expenseStore.TextChangedExp = expenseBook.Month.ToString() + "/" + expenseBook.Year.ToString();
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
