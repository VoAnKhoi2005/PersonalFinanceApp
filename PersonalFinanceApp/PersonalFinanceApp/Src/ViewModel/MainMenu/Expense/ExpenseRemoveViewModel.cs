using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows;
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
        try {
            var itemExp = DBManager.GetFirst<Expense>(e => _expenseStore.Expenses.UserID == e.UserID && _expenseStore.Expenses.ExpenseID == e.ExpenseID, getDeleted: true);
            DBManager.PermanentlyDelete(itemExp);
            _expenseStore.NotifyRecycle();
        }
        catch (Exception ex) {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        _modalNavigationStore.Close();
    }
}
