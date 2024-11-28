using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class ExpenseBookDeleteViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly ExpenseBookStore _expenseBookStore;
    #region Properties
    
    #endregion
    public ICommand CancelDeleteExpenseBookCommand { get; set; }
    public ICommand ConfirmDeleteExpenseBookCommand { get; set; }

    public ExpenseBookDeleteViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _expenseBookStore = serviceProvider.GetRequiredService<ExpenseBookStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        CancelDeleteExpenseBookCommand = new RelayCommand<object>(CloseModal);
        ConfirmDeleteExpenseBookCommand = new RelayCommand<object>(ConfirmExpenseBook);
    }
    
    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmExpenseBook(object sender) {
        //add data to database
        var itemExB = _expenseBookStore.ExpenseBook;
        ExpensesBook itemEdit = DBManager.GetFirst<ExpensesBook>(exb => exb.Month == itemExB.Month && exb.Year == itemExB.Year && exb.UserID == itemExB.UserID);
        DBManager.Remove<ExpensesBook>(itemEdit);
        _modalNavigationStore.Close();
    }
}
