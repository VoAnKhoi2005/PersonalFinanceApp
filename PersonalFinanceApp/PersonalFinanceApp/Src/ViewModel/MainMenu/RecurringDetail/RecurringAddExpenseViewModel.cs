using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Windows.UI.Notifications;

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
        try {
            var result = MessageBox.Show("Bạn có chắc không muốn thêm bất kỳ expense nào nữa chứ!", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes) {
                List<Expense> ex = _recurringStore.ShareExpense
                    .GroupBy(e => e.RecurringExpenseID)
                    .Select(g => g.First())
                    .ToList();
                foreach (var item in ex) {
                    var rec = DBManager.GetFirst<RecurringExpense>(re => re.UserID == usr.UserID && re.RecurringExpenseID == item.RecurringExpenseID);
                    if (rec.Frequency.CompareTo("Daily") == 0) {
                        rec.LastTime = rec.LastTime.AddDays(rec.Interval);
                    }
                    else if (rec.Frequency.CompareTo("Weekly") == 0) {
                        rec.LastTime = rec.LastTime.AddDays(rec.Interval * 7);
                    }
                    else if (rec.Frequency.CompareTo("Monthly") == 0) {
                        rec.LastTime = rec.LastTime.AddMonths(rec.Interval);
                    }
                    else if (rec.Frequency.CompareTo("Yearly") == 0) {
                        rec.LastTime = rec.LastTime.AddYears(rec.Interval);
                    }
                    DBManager.Update(rec);
                }
                _recurringStore.ShareExpense.Clear();
                _modalNavigationStore.Close();
            }
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
    }
    public void AddNewExpense(object? parameter = null) {
        Expense exp = new Expense(ItemRecurring.Amount, ItemRecurring.Name, DateOnly.FromDateTime(DateTime.Now),
                       ItemRecurring.CategoryID, ItemRecurring.ExBMonth, ItemRecurring.ExBYear, usr.UserID, ItemRecurring.Description);
        exp.RecurringExpenseID = ItemRecurring.RecurringExpenseID;
        DBManager.Insert(exp);

        SourceRecurringExpense.Remove(ItemRecurring);
    }

    public void CancelAddExpense(object? parameter = null) {
        var result = MessageBox.Show("Bạn có chắc không muốn thêm expense này!", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Information);
        if (result == MessageBoxResult.Yes) {
            SourceRecurringExpense.Remove(ItemRecurring);
        }
    }
    public void LoadData() {
        SourceRecurringExpense = _recurringStore.ShareExpense;
    }

}
