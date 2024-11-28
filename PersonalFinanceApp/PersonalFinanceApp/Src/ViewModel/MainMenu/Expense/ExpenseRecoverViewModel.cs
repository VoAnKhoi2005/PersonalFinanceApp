using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class ExpenseRecoverViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    public ObservableCollection<Expense> ExpensesRecover { 
        get => _expensesRecover;
        set {
            if(_expensesRecover != value) {
                _expensesRecover = value;
                OnPropertyChanged();
            }
        } 
    }
    private ObservableCollection<Expense> _expensesRecover;
    public ICommand RefreshRecoverExpenseCommand { get; set; }
    public bool HasNoExpenseDelete { get; set; } = true;
    public ExpenseRecoverViewModel(IServiceProvider serviceProvider) {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        RefreshRecoverExpenseCommand = new RelayCommand<object>(LoadExpenses);
    }
    public void LoadExpenses(object p) {
        //load data to datagrid
        //var items = DBManager.GetAll<Expense>();
        var items = DBManager.GetCondition<Expense>(exp => exp.Deleted == true);
        ExpensesRecover = new(items);
    }
}