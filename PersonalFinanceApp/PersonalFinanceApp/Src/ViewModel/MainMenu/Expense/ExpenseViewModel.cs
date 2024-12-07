using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.Model;
using System.Text.RegularExpressions;
using System.Windows;
using Windows.Devices.Power;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class ExpenseViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseStore _expenseStore;
    private readonly AccountStore _accountStore;
    #region Properties
    //not recover
    public bool IsButtonVisible {
        get => _isButtonVisible;
        set {
            _isButtonVisible = value;
            OnPropertyChanged();
        }
    }
    private bool _isButtonVisible;
    //recover
    public bool IsButtonRecover {
        get => _isButtonRecover;
        set {
            _isButtonRecover = value;
            OnPropertyChanged();
        }
    }
    private bool _isButtonRecover;


    //is number
    public bool IsNumber {
        get => _isNumber;
        set {
            if(value != _isNumber) {
                _isNumber = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _isNumber;
    //is string
    public bool IsString {
        get => _isString;
        set {
            if (value != _isString) {
                _isString = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _isString;
    //is date
    public bool IsDate {
        get => _isDate;
        set {
            if (value != _isDate) {
                _isDate = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _isDate;
    //type filter
    public string SelectedTypeFilter {
        get => _selectedTypeFilter;
        set {
            _selectedTypeFilter = value;
            OnPropertyChanged();
        }
    }
    private string _selectedTypeFilter;
    //source filter
    public ObservableCollection<string> SourceFilter {
        get => _sourceFilter;
        set {
            if (_sourceFilter != value) {
                _sourceFilter = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<string> _sourceFilter = new();
    //data filter
    public string DataFilter {
        get => _dataFilter;
        set {
            if (_dataFilter != value) {
                _dataFilter = value;
                OnPropertyChanged();
            }
        }
    }
    private string _dataFilter;


    //choose expense advance
    public ExpenseAdvance ItemsExB {
        get => _itemsExB;
        set {
            if (_itemsExB != value && value != null) {
                _itemsExB = value;
                _expenseStore.Expenses = value.exp;
                OnPropertyChanged();
            }
        }
    }
    private ExpenseAdvance _itemsExB;
    //source expense advance
    public ObservableCollection<ExpenseAdvance> Expensesadvances { 
        get => _expensesAdvances;
        set {
            if (_expensesAdvances != value) {
                _expensesAdvances = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<ExpenseAdvance> _expensesAdvances = new();
    //
    #endregion
    #region Command
    public ICommand AddNewExpenseCommand { get; set; }
    public ICommand RefreshExpenseCommand { get; set; }
    public ICommand EditExpenseCommand { get; set; }
    public ICommand DeleteExpenseCommand { get; set; }
    public ICommand RecoverExpenseCommand { get; set; }
    public ICommand RemoveExpenseCommand { get; set; }
    public ICommand FilterExpenseCommand { get; set; }
    public ICommand LoadRecoverCommand { get; set; }
    public ICommand RecurringDetailCommand { get; set; }
    public ICommand MatchWholeWordCommand {  get; set; }
    public ICommand MatchCaseCommand { get; set; }
    public ICommand MatchRegexCommand { get; set; }
    public ICommand FindNumberCommand { get; set; }
    public ICommand FindDateCommand { get; set; }
    public ICommand SelectionChangedCommand { get; set; }
    #endregion

    public ExpenseViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();

        AddNewExpenseCommand = new NavigateModalCommand<ExpenseAddNewViewModel>(serviceProvider);
        EditExpenseCommand = new NavigateModalCommand<ExpenseEditViewModel>(serviceProvider);
        DeleteExpenseCommand = new NavigateModalCommand<ExpenseDeleteViewModel>(serviceProvider);
        RemoveExpenseCommand = new NavigateModalCommand<ExpenseRemoveViewModel>(serviceProvider);
        RecoverExpenseCommand = new NavigateModalCommand<ExpenseRecoverViewModel>(serviceProvider);
        RecurringDetailCommand = new NavigateModalCommand<RecurringViewModel>(serviceProvider);

        LoadExpenses();

        LoadRecoverCommand = new RelayCommand<object>(LoadRecover);
        RefreshExpenseCommand = new RelayCommand<object>(LoadExpenses);

        SelectionChangedCommand = new RelayCommand<object>(SelectionChanged);
        FindNumberCommand = new RelayCommand<object>(FilterNumber);
        FindDateCommand = new RelayCommand<object>(FilterDate);
        MatchWholeWordCommand = new RelayCommand<object>(Matchwholeword);
        MatchCaseCommand = new RelayCommand<object>(MatchCase);
        MatchRegexCommand = new RelayCommand<object>(MatchRegex);

    }
    public void Filter(string pattern) {
        var userID = int.Parse(_accountStore.UsersID);
        Expensesadvances.Clear();

        var items = DBManager.GetCondition<Expense>(exp => exp.UserID == userID);
        if (items == null) return;

        if (SelectedTypeFilter.CompareTo("Name") == 0) {
            foreach (var item in items) {
                if (Regex.IsMatch(item.Name, pattern)) {
                    var exB = DBManager.GetFirst<ExpensesBook>(e => e.Month == item.ExBMonth && e.Year == item.ExBYear && e.UserID == item.UserID);
                    if (exB != null) Expensesadvances.Add(new ExpenseAdvance(item, exB.Budget));
                }
                else {
                    continue;
                }
            }
        }
        else if (SelectedTypeFilter.CompareTo("Description") == 0) {
            foreach (var item in items) {
                if (Regex.IsMatch(item.Description, pattern)) {
                    var exB = DBManager.GetFirst<ExpensesBook>(e => e.Month == item.ExBMonth && e.Year == item.ExBYear && e.UserID == item.UserID);
                    if (exB != null) Expensesadvances.Add(new ExpenseAdvance(item, exB.Budget));
                }
                else {
                    continue;
                }
            }
        }
        else {
            MessageBox.Show("Dữ liệu có vấn đề! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public void FilterDate(object? parameter = null) {
        if (DataFilter == null) {
            MessageBox.Show("Dữ liệu có vấn đề! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        string pattern = $@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$";
        if(!Regex.IsMatch(DataFilter, pattern)) {
            MessageBox.Show("Dữ liệu có vấn đề! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
        var datefilter = DataFilter.Split('/');
        var day = int.Parse(datefilter[0]);
        var month = int.Parse(datefilter[1]);
        var year = int.Parse(datefilter[2]);
        var date = new DateOnly(day, month, year);
        switch (month) {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                if (day > 31 || day < 1) {
                    MessageBox.Show("Ngày không phù hợp! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                break;
            case 2:
                if (year % 4 == 0 && year % 100 != 0) {
                    if(day > 29 || day < 1) {
                        MessageBox.Show("Ngày không phù hợp! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else {
                    if (day > 28 || day < 1) {
                        MessageBox.Show("Ngày không phù hợp! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                break;
            default:
                if (day > 30 || day < 1) {
                    MessageBox.Show("Ngày không phù hợp! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                break;
        }

        var userID = int.Parse(_accountStore.UsersID);
        Expensesadvances.Clear();

        var items = DBManager.GetCondition<Expense>(exp => exp.UserID == userID && exp.Date == date);
        if (items == null) return;
        foreach(var item in items) {
            var exB = DBManager.GetFirst<ExpensesBook>(e => e.Month == item.ExBMonth && e.Year == item.ExBYear && e.UserID == item.UserID);
            if (exB != null) Expensesadvances.Add(new ExpenseAdvance(item, exB.Budget));
        }
    }
    public void MatchRegex(object? parameter = null) {
        if (DataFilter == null) {
            MessageBox.Show("Dữ liệu có vấn đề! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        if (!IsRegexValid(DataFilter)) return;
        string pattern = $@"{DataFilter}";
        Filter(pattern);
    }
    public bool IsRegexValid(string pattern) {
        try {
            Regex regex = new Regex(pattern);
            return true;
        }
        catch (ArgumentException) {
            MessageBox.Show("Dữ liệu có vấn đề! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
    }
    public void MatchCase(object? parameter = null) {
        if (DataFilter == null) {
            MessageBox.Show("Dữ liệu có vấn đề! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        string pattern = $@"{Regex.Escape(DataFilter)}";
        Filter(pattern);
    }
    public void Matchwholeword(object? parameter = null) {
        if(DataFilter == null) {
            MessageBox.Show("Dữ liệu có vấn đề! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        string pattern = $@"\b{Regex.Escape(DataFilter)}\b";
        Filter(pattern);
    }
    public void FilterNumber(object? parameter = null) {
        string[] patterns = {
        @"^(<)([0-9])$",
        @"^(>)([0-9])$",
        @"^(=)([0-9])$",
        @"^(!)([0-9])$",
        @"^(<)(=)([0-9])$",
        @"^(>)(=)([0-9])$"
    };

        Func<string, long, Func<long, long, bool>>[] conditions = {
        (op, val) => (x, y) => x < y,
        (op, val) => (x, y) => x > y,
        (op, val) => (x, y) => x == y,
        (op, val) => (x, y) => x != y,
        (op, val) => (x, y) => x <= y,
        (op, val) => (x, y) => x >= y
    };

        string numberString = Regex.Match(DataFilter, "\\d+").Value;
        if (!long.TryParse(numberString, out long number)) {
            MessageBox.Show("Dữ liệu tìm kiếm quá to! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        Expensesadvances.Clear();
        for (int i = 0; i < patterns.Length; i++) {
            if (Regex.IsMatch(DataFilter, patterns[i])) {
                var condition = conditions[i]("", number);
                ApplyFilter(condition, number);
                return;
            }
        }

        MessageBox.Show("Sai định dạng rồi! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    private void ApplyFilter(Func<long, long, bool> condition, long number) {
        var userID = int.Parse(_accountStore.UsersID);

        if (SelectedTypeFilter.CompareTo("Amount") == 0) {
            var items = DBManager.GetCondition<Expense>(exp => exp.UserID == userID && condition(exp.Amount, number));
            AddExpensesAdvances(items);
        }
        else {
            var items = DBManager.GetCondition<Expense>(exp => exp.UserID == userID);
            if (items != null) {
                foreach (var item in items) {
                    var exB = DBManager.GetFirst<ExpensesBook>(e => e.Month == item.ExBMonth && e.Year == item.ExBYear && e.UserID == item.UserID && condition(e.Budget, number));
                    if (exB != null) Expensesadvances.Add(new ExpenseAdvance(item, exB.Budget));
                }
            }
        }
    }
    private void AddExpensesAdvances(IEnumerable<Expense> items) {
        if (items != null) {
            foreach (var item in items) {
                var exB = DBManager.GetFirst<ExpensesBook>(e => e.Month == item.ExBMonth && e.Year == item.ExBYear && e.UserID == item.UserID);
                if (exB != null) Expensesadvances.Add(new ExpenseAdvance(item, exB.Budget));
            }
        }
    }
    public void SelectionChanged(object? parameter = null) {
        if(SelectedTypeFilter.CompareTo("Amount") == 0 || SelectedTypeFilter.CompareTo("Budget") == 0) {
            IsNumber = true;
            IsString = false;
            IsDate = false;
        }
        else if(SelectedTypeFilter.CompareTo("Date") == 0){
            IsNumber = false;
            IsString = false;
            IsDate = true;
        }
        else {
            IsNumber = false;
            IsString = true;
            IsDate = false;
        }
    }
    public void LoadRecover(object parameter) {
        IsButtonVisible = false;
        IsButtonRecover = true;
        Expensesadvances.Clear();
        var exp = DBManager.GetCondition<Expense>(e => e.UserID == int.Parse(_accountStore.UsersID), getDeleted:true);
        foreach(var e in exp) {
            var exB = DBManager.GetFirst<ExpensesBook>(ex => ex.UserID == e.UserID && ex.Month == e.ExBMonth && ex.Year == e.ExBYear);
            Expensesadvances.Add(new ExpenseAdvance(e, exB.Budget));
        }

    }
    public void LoadExpenses(object? p = null) {
        IsButtonVisible = true;
        IsButtonRecover = false;
        //load data to datagrid
        Expensesadvances.Clear();
        var items = DBManager.GetCondition<Expense>(exp => exp.UserID == int.Parse(_accountStore.UsersID));
        foreach (var item in items) {
            var exB = DBManager.GetFirst<ExpensesBook>(e => e.Month == item.ExBMonth && e.Year == item.ExBYear && e.UserID == item.UserID);
            Expensesadvances.Add(new ExpenseAdvance(item, exB.Budget));
        }
        //Load data to Type filter
        SourceFilter.Clear();
        //string
        SourceFilter.Add("Date");
        SourceFilter.Add("Name");
        SourceFilter.Add("Description");
        //number
        SourceFilter.Add("Amount");
        SourceFilter.Add("Budget");
    }
    public class ExpenseAdvance {
        public Expense exp {  get; set; }
        public long BudgetExp { get; set; }
        public int ExpenseID { get; set; }
        public long Amount { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
        public bool Recurring { get; set; }
        public DateTime TimeAdded { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public ExpenseAdvance() { }
        public ExpenseAdvance(Expense ex, long budget) {
            exp = ex;
            BudgetExp = budget;
            ExpenseID = ex.ExpenseID;
            Amount = ex.Amount;
            Name = ex.Name;
            Description = ex.Description;   
            Date = ex.Date;
            TimeAdded = ex.TimeAdded;
            CategoryID = ex.CategoryID;
            var cate = DBManager.GetFirst<Category>(c => c.CategoryID == ex.CategoryID);
            Category = cate.Name;
        }
    }
}
