using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Services.Maps;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class RecurringAddExpenseViewModel : BaseViewModel {
    private readonly IServiceProvider _serviceProvider;
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly AccountStore _accountStore;
    private readonly RecurringStore _recurringStore;
    public User usr {  get; set; }
    //source recurring
    public ObservableCollection<Expense> SourceRecurringExpense {
        get => _sourceRecurringExpense;
        set {
            if(value != _sourceRecurringExpense) {
                _sourceRecurringExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<Expense> _sourceRecurringExpense = new();

    //item in datagrid
    public Expense ItemRecurring {
        get => _itemRecurring;
        set {
            if (value != _itemRecurring) {
                _itemRecurring = value;
                OnPropertyChanged();
            }
        }
    }
    private Expense _itemRecurring;

    public ICommand AddExpenseCommand { get; set; }
    public ICommand CancelAddExpenseCommand { get; set; }
    public ICommand CloseModalCommand { get; set; }
    public RecurringAddExpenseViewModel(IServiceProvider serviceProvider) { 
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _recurringStore = serviceProvider.GetRequiredService<RecurringStore>();

        usr = _accountStore.Users;

        LoadData();

        CloseModalCommand = new RelayCommand<object>(CloseModal);
        AddExpenseCommand = new RelayCommand<object>(AddNewExpense);
        CancelAddExpenseCommand = new RelayCommand<object>(CancelAddExpense);

    }
    public void CloseModal(object? parameter = null) {
        var result = MessageBox.Show("Bạn có chắc không muốn thêm bất kỳ expense nào nữa chứ!", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Information);
        if (result == MessageBoxResult.Yes) {
            _modalNavigationStore.Close();  
        }
    }
    public void AddNewExpense(object? parameter = null) {
        SourceRecurringExpense.Remove(ItemRecurring);
        Expense exp = new Expense(ItemRecurring.Amount, ItemRecurring.Name, DateOnly.FromDateTime(DateTime.Now),
                       ItemRecurring.CategoryID, ItemRecurring.ExBMonth, ItemRecurring.ExBYear, usr.UserID, ItemRecurring.Description);
        exp.RecurringExpenseID = ItemRecurring.RecurringExpenseID;
        DBManager.Insert(exp);
    }

    public void CancelAddExpense(object? parameter = null) {
        SourceRecurringExpense.Remove(ItemRecurring);
    }
    public void LoadData() {
        SourceRecurringExpense = _recurringStore.ShareExpense;
    }
}
