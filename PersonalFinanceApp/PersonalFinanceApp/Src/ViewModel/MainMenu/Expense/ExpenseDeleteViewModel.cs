using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.MainMenu;
using PersonalFinanceApp.ViewModel.Stores;
using PersonalFinanceApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PersonalFinanceApp.Src.ViewModel.MainMenu;

public class ExpenseDeleteViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    public ObservableCollection<Expense> ExpensesDelete { get; set; }
    public ICommand RefreshDeleteExpenseCommand { get; set; }
    public bool HasNoExpenseDelete { get; set; } = true;
    public ExpenseDeleteViewModel(IServiceProvider serviceProvider) {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        RefreshDeleteExpenseCommand = new RelayCommand<Object>(p => LoadDeleteExpenses(p));
    }
    public void LoadDeleteExpenses(object p) {
        //load data to datagrid
        //var items = DBManager.GetAll<Expense>();
        var items = DBManager.GetCondition<Expense>(exp => exp.Deleted == true);
        ExpensesDelete = new(items);
    }
}
