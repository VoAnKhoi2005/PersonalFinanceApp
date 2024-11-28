
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class ExpenseBookDeleteViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    #region Properties
    
    //itemsource month
    public ObservableCollection<Month> MonthsEdit {
        get => _monthsEdit;
        set {
            if (_monthsEdit != value) {
                _monthsEdit = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<Month> _monthsEdit;
    //itemsource year
    public ObservableCollection<int> YearsEdit {
        get => _yearsEdit;
        set {
            if (_yearsEdit != value) {
                _yearsEdit = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<int> _yearsEdit;
    //selected item month
    public Month SelectedMonthEdit {
        get => _selectedMonthEdit;
        set {
            if (_selectedMonthEdit != value) {
                _selectedMonthEdit = value;
                OnPropertyChanged();
            }
        }
    }
    private Month _selectedMonthEdit;
    //selected item year
    public int SelectedYearEdit {
        get => _selectedYearEdit;
        set {
            if (_selectedYearEdit != value) {
                _selectedYearEdit = value;
                OnPropertyChanged();
            }
        }
    }
    private int _selectedYearEdit;

    //budget
    public string BudgetExpenseBookEdit {
        get => _budgetExpenseBookEdit;
        set {
            if (_budgetExpenseBookEdit != value) {
                _budgetExpenseBookEdit = value;
                OnPropertyChanged();
            }
        }
    }
    private string _budgetExpenseBookEdit;
    //resource
    public string ResourceExpenseBookEdit {
        get => _resourceExpenseBookEdit;
        set {
            if (_resourceExpenseBookEdit != value) {
                _resourceExpenseBookEdit = value;
                OnPropertyChanged();
            }
        }
    }
    private string _resourceExpenseBookEdit;
    #endregion
    public ICommand CancelEditExpenseBookCommand { get; set; }
    public ICommand ConfirmEditExpenseBookCommand { get; set; }

    public ExpenseBookDeleteViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();

        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        LoadItem();
        CancelEditExpenseBookCommand = new RelayCommand<object>(CloseModal);
        ConfirmEditExpenseBookCommand = new RelayCommand<object>(ConfirmExpenseBook);
    }
    public void LoadItem() {
        MonthsEdit = new ObservableCollection<Month>{
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

        YearsEdit = new ObservableCollection<int>();
        for (int year = 2000; year <= 2030; year++) {
            YearsEdit.Add(year);
        }

    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmExpenseBook(object sender) {
        //add data to database
        var expenseBook = new ExpensesBook() {
            //Month = SelectedMonth.Number,
            //Year = SelectedYear,
            //Budget = long.Parse(BudgetExpenseBook),
            //Resources = ResourceExpenseBook,
        };
        DBManager.Update<ExpensesBook>(expenseBook);
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
