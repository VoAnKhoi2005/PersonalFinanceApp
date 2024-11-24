using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PersonalFinanceApp.Src.ViewModel.MainMenu;

public class ExpenseBookEditViewModel : BaseViewModel
{
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    #region Properties
    //month
    public string MonthEditExpenseBook {
        get => _monthEditExpenseBook;
        set {
            if (_monthEditExpenseBook != value) {
                _monthEditExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _monthEditExpenseBook;
    //year
    public string YearEditExpenseBook {
        get => _yearEditExpenseBook;
        set {
            if (_yearEditExpenseBook != value) {
                _yearEditExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _yearEditExpenseBook;
    //budget
    public string BudgetEditExpenseBook {
        get => _budgetEditExpenseBook;
        set {
            if (_budgetEditExpenseBook != value) {
                _budgetEditExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _budgetEditExpenseBook;
    //resource
    public string ResourceEditExpenseBook {
        get => _resourceEditExpenseBook;
        set {
            if (_resourceEditExpenseBook != value) {
                _resourceEditExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _resourceEditExpenseBook;
    #endregion
    public ICommand CancelEditExpenseBookCommand { get; set; }
    public ICommand ConfirmEditExpenseBookCommand { get; set; }

    public ExpenseBookEditViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        CancelEditExpenseBookCommand = new RelayCommand<object>(CloseModal);
        ConfirmEditExpenseBookCommand = new RelayCommand<object>(ConfirmEditExpenseBook);
    }

    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmEditExpenseBook(object sender) {
        //add data to database
        var expenseBook = new ExpensesBook() {
            //Month = int.Parse(MonthEditExpenseBook),
            //Year = int.Parse(YearEditExpenseBook),
            //Budget = long.Parse(BudgetEditExpenseBook),
            //Resources = ResourceEditExpenseBook,
        };
        DBManager.Insert(expenseBook);
        _modalNavigationStore.Close();
    }
}
