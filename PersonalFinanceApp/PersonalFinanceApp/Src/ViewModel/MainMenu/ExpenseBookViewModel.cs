using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;


namespace PersonalFinanceApp.ViewModel.MainMenu; 
internal class ExpenseBookViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    public ObservableCollection<ExpensesBook> ExpenseBooks {get; set;}
    public ICommand AddNewExpenseBookCommand { get; set; }
    public ICommand RefreshExpenseBookCommand { get; set; }
    public bool HasNoExpenseBook { get; set; } = true;

    public ExpenseBookViewModel(IServiceProvider serviceProvider) {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        AddNewExpenseBookCommand = new NavigateModalCommand<ExpenseBookAddNewViewModel>(serviceProvider);

        RefreshExpenseBookCommand = new RelayCommand<Object>(p => LoadExpenseBooks(p));
    }
    public void LoadExpenseBooks(object p) {
        //load data to datagrid
        var items = DBManager.GetAll<ExpensesBook>();
        ExpenseBooks = new(items);
    }

}
