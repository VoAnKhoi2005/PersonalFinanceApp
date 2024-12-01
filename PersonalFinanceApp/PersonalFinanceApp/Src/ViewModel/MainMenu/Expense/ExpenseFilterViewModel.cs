using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu; 
class ExpenseFilterViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly ExpenseStore _expenseStore;
    private readonly SharedService _sharedService;
    #region Properties
    //isday
    public bool IsDay {
        get => _isDay;
        set {
            if (_isDay != value) {
                _isDay = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _isDay;
    //selected options
    public string Option {
        get => _option;
        set {
            if (_option != value) {
                _option = value;
                if (value.CompareTo("Expense") == 0) {
                    IsDay = true;
                }
                else IsDay = false;
                OnPropertyChanged(nameof(Option));
            }
        }
    }
    private string _option;
    //selected day
    public string Day {
        get => _day;
        set {
            if (_day != value) {
                _day = value;
                OnPropertyChanged();
            }
        }
    }
    private string _day;
    //selected month
    public string Month {
        get => _month;
        set {
            if (_month != value) {
                _month = value;
                OnPropertyChanged();
            }
        }
    }
    private string _month;
    //selected year
    public string Year {
        get => _year;
        set {
            if (_year != value) {
                _year = value;
                OnPropertyChanged();
            }
        }
    }
    private string _year;
    //source
    public ObservableCollection<string> SourceDay {
        get => _sourceDay;
        set {
            if (_sourceDay != value) {
                _sourceDay = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<string> _sourceDay = new();
    public ObservableCollection<string> SourceMonth {
        get => _sourceMonth;
        set {
            if (_sourceMonth != value) {
                _sourceMonth = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<string> _sourceMonth = new();
    public ObservableCollection<string> SourceYear {
        get => _sourceYear;
        set {
            if (_sourceYear != value) {
                _sourceYear = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<string> _sourceYear = new();
    public ObservableCollection<string> TypeFilter {
        get => _typeFilter;
        set {
            if (_typeFilter != value) {
                _typeFilter = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<string> _typeFilter = new();
    #endregion
    public ICommand CancelFilterCommand { get; set; }
    public ICommand ConfirmFilterCommand { get; set; }

    public ExpenseFilterViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _sharedService = serviceProvider.GetRequiredService<SharedService>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        LoadItemsSource();
        CancelFilterCommand = new RelayCommand<object>(CloseModal);
        ConfirmFilterCommand = new RelayCommand<object>(ConfirmModal);
    }
    public void LoadItemsSource(object? parameter = null) {
        SourceDay.Clear();
        for(int i = 1; i <= 31; ++i) {
            SourceDay.Add(i.ToString());
        }
        SourceMonth.Clear();
        for(int i = 1; i <= 12; ++i) {
            SourceMonth.Add(i.ToString());
        }
        SourceYear.Clear();
        for(int i = 2000; i <= DateTime.Now.Year; ++i) {
            SourceYear.Add(i.ToString());
        }
        TypeFilter.Clear();
        TypeFilter.Add("Expense");
        TypeFilter.Add("Expense Book");
        TypeFilter.Add("Expense Deleted");
        TypeFilter.Add("Expense Book Deleted");
    }
    public void CloseModal(object parameter) {
        _modalNavigationStore.Close();
    }
    public void ConfirmModal(object parameter) {
        //confirm
        
        if (Option.CompareTo("Expense") == 0) {
            IsDay = true;
            if (Day == null || Month == null || Year == null) _modalNavigationStore.Close();
            var items = DBManager.GetCondition<Expense>(e => e.UserID == int.Parse(_accountStore.UsersID) && e.Date.Day == int.Parse(Day) && e.Date.Month == int.Parse(Month) && e.Date.Year == int.Parse(Year));
            _expenseStore.SharedExpense.Clear();
            foreach(var item in items) {
                var exb = DBManager.GetFirst<ExpensesBook>(e => e.Month == item.ExBMonth && e.Year == item.ExBYear && e.UserID == item.UserID);
                _expenseStore.SharedExpense.Add(new ExpenseStore.ExpenseAdvance(item, exb.Budget));
            }
        }
        else if (Option.CompareTo("Expense Book") == 0) {
            IsDay = false;
            if (Month == null || Year == null) _modalNavigationStore.Close();
            var items = DBManager.GetCondition<Expense>(e => e.UserID == int.Parse(_accountStore.UsersID) && e.ExBMonth == int.Parse(Month) && e.ExBYear == int.Parse(Year));
            _expenseStore.SharedExpense.Clear();
            foreach (var item in items) {
                var exb = DBManager.GetFirst<ExpensesBook>(e => e.Month == item.ExBMonth && e.Year == item.ExBYear && e.UserID == item.UserID);
                _expenseStore.SharedExpense.Add(new ExpenseStore.ExpenseAdvance(item, exb.Budget));
            }
        }
        else if (Option.CompareTo("Expense Deleted") == 0) {
            IsDay = true;
            if (Day == null || Month == null || Year == null) _modalNavigationStore.Close();
            var items = DBManager.GetCondition<Expense>(e => e.UserID == int.Parse(_accountStore.UsersID) && e.Date.Day == int.Parse(Day) && e.Date.Month == int.Parse(Month) && e.Date.Year == int.Parse(Year), getDeleted:true);
            _expenseStore.SharedExpense.Clear();
            foreach (var item in items) {
                var exb = DBManager.GetFirst<ExpensesBook>(e => e.Month == item.ExBMonth && e.Year == item.ExBYear && e.UserID == item.UserID);
                _expenseStore.SharedExpense.Add(new ExpenseStore.ExpenseAdvance(item, exb.Budget));
            }
        }
        else {
            IsDay = false;
            if (Month == null || Year == null) _modalNavigationStore.Close();
            var items = DBManager.GetCondition<Expense>(e => e.UserID == int.Parse(_accountStore.UsersID) && e.ExBMonth == int.Parse(Month) && e.ExBYear == int.Parse(Year), getDeleted: true);
            _expenseStore.SharedExpense.Clear();
            foreach (var item in items) {
                var exb = DBManager.GetFirst<ExpensesBook>(e => e.Month == item.ExBMonth && e.Year == item.ExBYear && e.UserID == item.UserID);
                _expenseStore.SharedExpense.Add(new ExpenseStore.ExpenseAdvance(item, exb.Budget));
            }
        }
        _sharedService.Notify();
        _modalNavigationStore.Close();
    }
   
}
