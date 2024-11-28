using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.MainMenu;
using PersonalFinanceApp.ViewModel.Stores;
using PersonalFinanceApp.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PersonalFinanceApp.Database;

namespace PersonalFinanceApp.Src.ViewModel.MainMenu;
public class ExpenseViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
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

    public bool HasNoExpense { get; set; } = true;
    public ExpenseViewModel(IServiceProvider serviceProvider) {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        AddNewExpenseCommand = new NavigateModalCommand<ExpenseAddNewViewModel>(serviceProvider);
        EditExpenseCommand = new NavigateModalCommand<ExpenseEditViewModel>(serviceProvider);

        RefreshExpenseCommand = new RelayCommand<object>(LoadExpenses);
    }
    public void LoadExpenses(object p) {
        //load data to datagrid
        //var items = DBManager.GetAll<Expense>();
        var items = DBManager.GetCondition<Expense>(exp => exp.Deleted == false);
        Expenses = new(items);
    }
}
