using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class ExpenseRemoveViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseStore _expenseStore;
    public ICommand CancelRemoveExpenseCommand { get; set; }
    public ICommand ConfirmRemoveExpenseCommand { get; set; }
    public ExpenseRemoveViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        CancelRemoveExpenseCommand = new RelayCommand<object>(CloseModal);
        ConfirmRemoveExpenseCommand = new RelayCommand<object>(ConfirmRemoveExpense);
    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmRemoveExpense(object sender) {
        //add data to database
        var itemExp = DBManager.GetFirst<Expense>(e => _expenseStore.Expenses.UserID == e.UserID && _expenseStore.Expenses.ExpenseID == e.ExpenseID, getDeleted: true);
        DBManager.Remove<Expense>(itemExp);
        _modalNavigationStore.Close();
    }
}
