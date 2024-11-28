using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class ExpenseBookEditViewModel : BaseViewModel
{
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly ExpenseBookStore _expenseBookStore;
    #region Properties
    //month
    public string MonthEditExpenseBook {
        get => _monthEditExpenseBook;
        set {
            if (_monthEditExpenseBook != value) {
                _monthEditExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _monthEditExpenseBook;
    //year
    public string YearEditExpenseBook {
        get => _yearEditExpenseBook;
        set {
            if (_yearEditExpenseBook != value) {
                _yearEditExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _yearEditExpenseBook;
    //budget
    public string BudgetEditExpenseBook {
        get => _budgetEditExpenseBook;
        set {
            if (_budgetEditExpenseBook != value) {
                _budgetEditExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _budgetEditExpenseBook;
    //resource
    public string ResourceEditExpenseBook {
        get => _resourceEditExpenseBook;
        set {
            if (_resourceEditExpenseBook != value) {
                _resourceEditExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _resourceEditExpenseBook;
    #endregion
    public ICommand CancelEditExpenseBookCommand { get; set; }
    public ICommand ConfirmEditExpenseBookCommand { get; set; }

    public ExpenseBookEditViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _expenseBookStore = serviceProvider.GetRequiredService<ExpenseBookStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        LoadData();
        CancelEditExpenseBookCommand = new RelayCommand<object>(CloseModal);
        ConfirmEditExpenseBookCommand = new RelayCommand<object>(ConfirmEditExpenseBook);
    }
    public void LoadData() {
        var itemExB = _expenseBookStore.ExpenseBook;
        if (itemExB != null) {
            MonthEditExpenseBook = itemExB.Month.ToString();
            YearEditExpenseBook = itemExB.Year.ToString();
            BudgetEditExpenseBook = itemExB.Budget.ToString();
            ResourceEditExpenseBook = itemExB.Resources; 
        }
    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmEditExpenseBook(object sender) {
        //add data to database
        var itemExB = _expenseBookStore.ExpenseBook;
        ExpensesBook itemEdit = DBManager.GetFirst<ExpensesBook>(exb => exb.Month == itemExB.Month && exb.Year == itemExB.Year && exb.UserID == itemExB.UserID);
        
        itemEdit.Month = int.Parse(MonthEditExpenseBook);
        itemEdit.Year = int.Parse(YearEditExpenseBook);
        itemEdit.Budget = long.Parse(BudgetEditExpenseBook);
        itemEdit.Resources = ResourceEditExpenseBook;

        bool check = DBManager.Update<ExpensesBook>(itemEdit);

        _modalNavigationStore.Close();
    }
}
