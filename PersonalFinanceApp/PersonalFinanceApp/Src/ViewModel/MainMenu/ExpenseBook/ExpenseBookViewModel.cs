using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.MainMenu;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;


namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class ExpenseBookViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly ExpenseBookStore _expenseBookStore;

    #region Properties
    public string ID {
        get => _iD;
        set {
            if (_iD != value) {
                _iD = value;
                OnPropertyChanged();
            }
        }
    }
    private string _iD;

    public ObservableCollection<ExpensesBook> ExpenseBooks {
        get => _expenseBooks;
        set {
            if (_expenseBooks != value) {
                _expenseBooks = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<ExpensesBook> _expenseBooks = new();
    public ICommand AddNewExpenseBookCommand { get; set; }
    public ICommand EditExpenseBookCommand { get; set; }
    public ICommand DeleteExpenseBookCommand { get; set; }
    public ICommand LoadExpenseBookCommand { get; set; }
    public ICommand AddExpenseCommand { get; set; }
    public ICommand RefreshExpenseBookCommand { get; set; }
    #endregion

    public ExpenseBookViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _expenseBookStore = serviceProvider.GetRequiredService<ExpenseBookStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        AddNewExpenseBookCommand = new NavigateModalCommand<ExpenseBookAddNewViewModel>(serviceProvider);

        EditExpenseBookCommand = new NavigateModalCommand<ExpenseBookEditViewModel>(serviceProvider);
        
        DeleteExpenseBookCommand = new NavigateModalCommand<ExpenseBookDeleteViewModel>(serviceProvider);
        AddExpenseCommand = new NavigateModalCommand<ExpenseViewModel>(serviceProvider);

        LoadExpenseBookCommand = new RelayCommand<object>(LoadExpenseBooks);
        RefreshExpenseBookCommand = new RelayCommand<object>(LoadExpenseBooks);
    }
    public void LoadExpenseBooks(object p) {
        //load data to datagrid
        ExpenseBooks.Clear();
        var items = DBManager.GetCondition<ExpensesBook>(exb => exb.UserID == int.Parse(_accountStore.UsersID));
        
        ExpenseBooks = new(items);
    }
    public void SaveId (object paramter) {
        _expenseBookStore.IdExB = ID;
    }
}
