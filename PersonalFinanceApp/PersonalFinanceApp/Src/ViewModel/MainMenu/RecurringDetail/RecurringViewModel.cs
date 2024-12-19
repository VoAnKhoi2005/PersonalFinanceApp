using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using XAct;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class RecurringViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly RecurringStore _recurringStore;

    public CalendarViewModel calendarViewModel { get; set; }
    public ObservableCollection<RecurringExpense> SourceRecurringExpense {
        get => _sourceRecurringExpense;
        set {
            if (value != _sourceRecurringExpense) {
                _sourceRecurringExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<RecurringExpense> _sourceRecurringExpense;

    public RecurringExpense SelectedRecurringExpense {
        get => _selectedRecurringExpense;
        set {
            if (value != _selectedRecurringExpense) {
                _selectedRecurringExpense = value;
                OnPropertyChanged();
            }
        }
    }
    private RecurringExpense _selectedRecurringExpense;

    public ICommand AddNewRecurringCommand { get; set; }
    public ICommand RefreshRecurringCommand { get; set;}
    public ICommand RecurringExpenseCommand { get; set; }

    public RecurringViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _recurringStore = serviceProvider.GetRequiredService<RecurringStore>();

        calendarViewModel = new CalendarViewModel(serviceProvider);

        RecurringExpenseCommand = new NavigateModalCommand<RecurringAddExpenseViewModel>(serviceProvider);

        LoadData();

        AddNewRecurringCommand = new NavigateModalCommand<RecurringAddnew>(serviceProvider);
    }

    public void LoadData(object? parameter = null) {
        SourceRecurringExpense = new();
        var items = DBManager.GetCondition<RecurringExpense>(re => re.UserID == _accountStore.Users.UserID);
        foreach (var item in items) {
            SourceRecurringExpense.Add(item);
        }
        AddRecurringExpense();

        if (_recurringStore.ShareExpense.Count != 0) RecurringExpenseCommand.Execute(this);
    }
    public void AddRecurringExpense() {
        try {
            DateTime date = DateTime.Today;
            var recs = DBManager.GetCondition<RecurringExpense>(re => re.UserID == int.Parse(_accountStore.UsersID));
            foreach (var rec in recs) {
                rec.LastTime = CatchNow(rec);

                //update database
                bool check = DBManager.Update(rec);
                if (!check) {
                    throw new Exception("Update Recurring LastTime Failed");
                }
            }
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    }
    public DateOnly CatchNow(RecurringExpense rec) {
        try {
            rec.Expenses = DBManager.GetCondition<Expense>(ex => ex.UserID == rec.UserID && ex.RecurringExpenseID == rec.RecurringExpenseID);
            Expense exp = rec.Expenses.OrderByDescending(e => e.Date).First();
            while (rec.LastTime < DateOnly.FromDateTime(DateTime.Today)) {
                if (exp != null && !exp.Date.Equals(rec.LastTime))
                    _recurringStore.ShareExpense.Add(new Expense(exp.Amount, exp.Name, rec.LastTime, exp.CategoryID,
                    exp.ExBMonth, exp.ExBYear, exp.UserID, rec.RecurringExpenseID, exp.Description));
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
            }
            exp = rec.Expenses.OrderByDescending(e => e.Date).First();
            if (rec.LastTime.Equals(DateOnly.FromDateTime(DateTime.Today)) && !rec.StartDate.Equals(DateOnly.FromDateTime(DateTime.Today))) {
                if (exp != null && !exp.Date.Equals(rec.LastTime)) _recurringStore.ShareExpense.Add(new Expense(exp.Amount, exp.Name, rec.LastTime, exp.CategoryID,
                    exp.ExBMonth, exp.ExBYear, exp.UserID, rec.RecurringExpenseID, exp.Description));
            }
            return rec.LastTime;
        }
        catch(Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return rec.LastTime;
        }
        
    }
}
