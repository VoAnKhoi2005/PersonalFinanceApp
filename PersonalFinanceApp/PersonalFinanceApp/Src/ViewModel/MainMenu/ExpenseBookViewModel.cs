using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;


namespace PersonalFinanceApp.ViewModel.MainMenu; 
internal class ExpenseBookViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;

    public ObservableCollection<Expense> Expenses {get; set;}


    public ICommand AddNewExpenseBookCommand { get; set; }
    public ICommand RefreshCommand { get; set; }
    public bool HasNoExpense { get; set; } = true;

    public ExpenseBookViewModel(IServiceProvider serviceProvider) {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        AddNewExpenseBookCommand = new NavigateModalCommand<ExpenseBookAddNewViewModel>(serviceProvider);

        Expenses = new ObservableCollection<Expense>();
        RefreshCommand = new RelayCommand<Object>(p => AddExpense(p));
    }


    public void AddExpense(object p) {

        //Expenses = new(items);


    }

}
