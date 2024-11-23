
﻿using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
internal class ExpenseBookAddNewViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly SharedDataService _sharedDataService;
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
    public ICommand ExpenseBookCancelCommand { get; set; }
    public ICommand ExpenseBookConfirmCommand { get; set; }

    public ExpenseBookAddNewViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _sharedDataService = serviceProvider.GetRequiredService<SharedDataService>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        ExpenseBookCancelCommand = new RelayCommand<object>(CloseModal);
        ExpenseBookConfirmCommand = new RelayCommand<object>(Confirm);
    }

    private void CloseModal(object sender) {
        _modalNavigationStore.Close();
    }
    private void Confirm(object sender) {
        //add data to database
        var expenseBook = new ExpensesBook() {
            Month = int.Parse(MonthExpenseBook),
            Year = int.Parse(YearExpenseBook),
            //UserID = userId;
            Budget = long.Parse(BudgetExpenseBook),
            Resources = ResourceExpenseBook,
        };
        DBManager.Insert(expenseBook);
        _modalNavigationStore.Close();
    }
}
