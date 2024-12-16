using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows;
using System.Windows.Input;
using XAct.Messages;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class ExpenseRecoverViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseStore _expenseStore;
    public ICommand CancelRecoverExpenseCommand { get; set; }
    public ICommand ConfirmRecoverExpenseCommand { get; set; }
    public ExpenseRecoverViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        CancelRecoverExpenseCommand = new RelayCommand<object>(CloseModal);
        ConfirmRecoverExpenseCommand = new RelayCommand<object>(ConfirmRecoverExpense);
    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmRecoverExpense(object sender) {
        //add data to database
        try {
            var itemExp = DBManager.GetFirst<Expense>(e => _expenseStore.Expenses.UserID == e.UserID && _expenseStore.Expenses.ExpenseID == e.ExpenseID, getDeleted: true);
            if (itemExp != null) {
                itemExp.Deleted = false;
            }
            DBManager.Update<Expense>(itemExp);
            _expenseStore.NotifyRecycle();
        }
        catch (Exception ex) {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _modalNavigationStore.Close();
    }
}