
﻿using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
internal class ExpenseBookAddNewViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly SharedDataService _sharedDataService;
    private string _typeExpenseBook;
    public string TypeExpenseBook {
        get => _typeExpenseBook;
        set {
            if (_typeExpenseBook != value) {
                _typeExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _amountExpenseBook;
    public string AmountExpenseBook {
        get => _amountExpenseBook;
        set {
            if (_amountExpenseBook != value) {
                _amountExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _nameExpenseBook;
    public string NameExpenseBook {
        get => _nameExpenseBook;
        set {
            if (_nameExpenseBook != value) {
                _nameExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _methodExpenseBook;
    public string MethodExpenseBook {
        get => _methodExpenseBook;
        set {
            if (_methodExpenseBook != value) {
                _methodExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _categoryExpenseBook;
    public string CategoryExpenseBook {
        get => _categoryExpenseBook;
        set {
            if (_categoryExpenseBook != value) {
                _categoryExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _dateTimeExpenseBook;
    public string DateTimeExpenseBook {
        get => _dateTimeExpenseBook;
        set {
            if (_dateTimeExpenseBook != value) {
                _dateTimeExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _statusExpenseBook;
    public string StatusExpenseBook {
        get => _statusExpenseBook;
        set {
            if (_statusExpenseBook != value) {
                _statusExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
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
        var exp = new Expense(5, "honguyentailoi");
        DBManager.Insert(exp);
        _modalNavigationStore.Close();
    }
}
