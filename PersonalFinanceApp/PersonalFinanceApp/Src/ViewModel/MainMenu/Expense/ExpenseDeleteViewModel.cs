using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class ExpenseDeleteViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseStore _expenseStore;
    private readonly SharedService _sharedService;
    public ICommand CancelDeleteExpenseCommand { get; set; }
    public ICommand ConfirmDeleteExpenseCommand { get; set; }
    public ExpenseDeleteViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _sharedService = serviceProvider.GetRequiredService<SharedService>();

        CancelDeleteExpenseCommand = new RelayCommand<object>(CloseModal);
        ConfirmDeleteExpenseCommand = new RelayCommand<object>(ConfirmDeleteExpense);
    }
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmDeleteExpense(object sender) {
        //add data to database
        try {
            var itemExp = DBManager.GetFirst<Expense>(e => _expenseStore.Expenses.UserID == e.UserID && _expenseStore.Expenses.ExpenseID == e.ExpenseID);
            if (itemExp != null) {
                itemExp.Deleted = true;
            }
            DBManager.Update<Expense>(itemExp);
            _sharedService.Notify();
        }
        catch(Exception ex) {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        _modalNavigationStore.Close();
    }
}
