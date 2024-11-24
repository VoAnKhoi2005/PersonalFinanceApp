
﻿using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class ExpenseBookAddNewViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    #region Properties
    //month
    public string MonthExpenseBook {
        get => _monthExpenseBook;
        set {
            if (_monthExpenseBook != value) {
                _monthExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _monthExpenseBook;
    //year
    public string YearExpenseBook {
        get => _yearExpenseBook;
        set {
            if (_yearExpenseBook != value) {
                _yearExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _yearExpenseBook;
    //budget
    public string BudgetExpenseBook {
        get => _budgetExpenseBook;
        set {
            if (_budgetExpenseBook != value) {
                _budgetExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _budgetExpenseBook;
    //resource
    public string ResourceExpenseBook {
        get => _resourceExpenseBook;
        set {
            if (_resourceExpenseBook != value) {
                _resourceExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _resourceExpenseBook;
    #endregion
    public ICommand CancelExpenseBookCommand { get; set; }
    public ICommand ConfirmExpenseBookCommand { get; set; }

    public ExpenseBookAddNewViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        CancelExpenseBookCommand = new RelayCommand<object>(CloseModal);
        ConfirmExpenseBookCommand = new RelayCommand<object>(ConfirmExpenseBook);
    }

    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void ConfirmExpenseBook(object sender) {
        //add data to database
        var expenseBook = new ExpensesBook() {
            //Month = int.Parse(MonthExpenseBook),
            //Year = int.Parse(YearExpenseBook),
            //Budget = long.Parse(BudgetExpenseBook),
            //Resources = ResourceExpenseBook,
        };
        DBManager.Insert(expenseBook);
        _modalNavigationStore.Close();
    }
}
