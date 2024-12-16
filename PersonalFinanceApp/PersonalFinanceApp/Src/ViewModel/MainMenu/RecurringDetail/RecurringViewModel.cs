using Microsoft.Extensions.DependencyInjection;
using OpenTK.Platform.Windows;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
public class RecurringViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly RecurringStore _recurringStore;
    public ObservableCollection<RecurringExpense> SourceRecurring {
        get => _sourceRecurring;
        set {
            if (_sourceRecurring != value) {
                _sourceRecurring = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<RecurringExpense> _sourceRecurring = new();
    public RecurringExpense ItemsRecurring {
        get => _itemsRecurring;
        set {
            if (_itemsRecurring != value) {
                _itemsRecurring = value;
                OnPropertyChanged();
            }
        }
    }
    private RecurringExpense _itemsRecurring;

    public CalendarViewModel calendarViewModel { get; set; }

    public ICommand AddNewRecurringCommand { get; set; }
    public ICommand RefreshRecurringCommand { get; set;}
    public ICommand RecurringExpenseCommand { get; set; }

    public RecurringViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _recurringStore = serviceProvider.GetRequiredService<RecurringStore>();

        //TestAddRecurring();

        calendarViewModel = new CalendarViewModel(serviceProvider);

        LoadData();
        //RecurringExpenseCommand = new NavigateModalCommand<>

        AddNewRecurringCommand = new NavigateModalCommand<RecurringAddnew>(serviceProvider);
        RefreshRecurringCommand = new RelayCommand<object>(LoadData);
    }
    public void TestAddRecurring(object? parameter = null) {
        var recurring = new RecurringExpense("ahii", "Daily", 1, new DateOnly(2024, 12, 15), int.Parse(_accountStore.UsersID));
        DBManager.Insert(recurring);
    }
    public void LoadData(object? parameter = null) {
        AddRecurringExpense();
    }
    public void AddRecurringExpense() {
        try {
            DateTime date = DateTime.Today;
            SourceRecurring.Clear();
            var recs = DBManager.GetCondition<RecurringExpense>(re => re.UserID == int.Parse(_accountStore.UsersID));
            foreach (var rec in recs) {
                rec.LastTime = CatchNow(rec);

                //if (rec.LastTime.Equals(DateOnly.FromDateTime(date))) {
                //    SourceRecurring.Add(rec);
                //}

                //update database
                //DBManager.Update(rec);
            }
            //_recurringStore.ShareRecurring = SourceRecurring;
            //navigate.excute
            //if(info.Count != 0) MessageBox.Show($"Co {info.Count} expense ban co muon them ?");
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    }    
    public DateOnly CatchNow(RecurringExpense rec) {
        Expense exp = DBManager.GetFirst<Expense>(ex => ex.UserID == rec.UserID && ex.RecurringExpenseID == rec.RecurringExpenseID); ;
        while(rec.LastTime < DateOnly.FromDateTime(DateTime.Today)) {
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
            _recurringStore.ShareExpense.Add(new Expense(exp.Amount, exp.Name, rec.LastTime, exp.CategoryID, exp.ExBMonth, exp.ExBYear, exp.UserID, exp.Description));
        }
        return rec.LastTime;
    }
}
