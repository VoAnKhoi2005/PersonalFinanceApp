﻿using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class ExpenseDeleteViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    public ObservableCollection<Expense> ExpensesDelete {
        get => _expensesDelete;
        set {
            if(_expensesDelete != value) {
                _expensesDelete = value;
                OnPropertyChanged();
            }
        } 
    }
    private ObservableCollection<Expense> _expensesDelete;
    public ICommand RefreshDeleteExpenseCommand { get; set; }
    public bool HasNoExpenseDelete { get; set; } = true;
    public ExpenseDeleteViewModel(IServiceProvider serviceProvider) {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        RefreshDeleteExpenseCommand = new RelayCommand<object>(LoadDeleteExpenses);
    }
    public void LoadDeleteExpenses(object p) {
        //load data to datagrid
        //var items = DBManager.GetAll<Expense>();
        var items = DBManager.GetCondition<Expense>(exp => exp.Deleted == true);
        ExpensesDelete = new(items);
    }
}
