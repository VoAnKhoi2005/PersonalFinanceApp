﻿using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.Model;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class ExpenseViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly ExpenseStore _expenseStore;
    private readonly AccountStore _accountStore;
    private readonly SharedService _sharedService;
    #region Properties
    //have expenseBook
    public bool HaveExpenseBook {
        get => _haveExpenseBook;
        set {
            if (_haveExpenseBook != value) {
                _haveExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _haveExpenseBook;
    //FILTER
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
    //text type
    public string TextType {
        get => _textType;
        set {
            if (_textType != value) {
                _textType = value;
                OnPropertyChanged();
            }
        }
    }
    private string _textType;


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
    //source category 
    public ObservableCollection<CheckBoxCategory> SourceCategory {
        get => _sourceCategory;
        set {
            if (_sourceCategory != value) {
                _sourceCategory = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<CheckBoxCategory> _sourceCategory = new();
    //source category true
    public ObservableCollection<CheckBoxCategory> CategoryFilter {
        get => _categoryFilter;
        set {
            if (_categoryFilter != value) {
                _categoryFilter = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<CheckBoxCategory> _categoryFilter = new();


    //EXPENS BOOK
    public ObservableCollection<ExpenseBookAdvance> SourceExpenseBook {
        get => _sourceExpenseBook;
        set {
            if (_sourceExpenseBook != value) {
                _sourceExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<ExpenseBookAdvance> _sourceExpenseBook = new();

    //selected exb
    public ExpenseBookAdvance SelectedExpenseBook {
        get => _selectedExpenseBook;
        set {
            if (_selectedExpenseBook != value) {
                if (value != null && value.DateExB.CompareTo("<New>") == 0) {
                    //excute new expensebook
                    NewExpenseBookCommand.Execute(this);
                }
                if(value != null) {
                    if (value.expensesBook != null) {
                        _expenseStore.ExpenseBook = value.expensesBook;
                    }
                }
                _selectedExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private ExpenseBookAdvance _selectedExpenseBook;
    //text exb
    public string TextExpenseBook {
        get => _textExpenseBook;
        set {
            if (_textExpenseBook != value) {
                _textExpenseBook = value;
                OnPropertyChanged();
            }
        }
    }
    private string _textExpenseBook;
    //budget expensebook official
    public string BudgetOfficial {
        get => _budgetOfficial;
        set {
            if (_budgetOfficial != value) {
                _budgetOfficial = value;
                OnPropertyChanged();
            }
        }
    }
    private string _budgetOfficial;
    //budget expense current
    public string BudgetCurrent {
        get => _budgetCurrent;
        set {
            if (value != _budgetCurrent) {
                _budgetCurrent = value;
                OnPropertyChanged();
            }
        }
    }
    private string _budgetCurrent;
    //all checked
    private bool _isAllChecked;

    public bool IsAllChecked {
        get => _isAllChecked;
        set {
            if (_isAllChecked != value) {
                _isAllChecked = value;
                OnPropertyChanged(nameof(IsAllChecked));
                UpdateItemsCheckState();
                OnPropertyChanged(nameof(SourceCategory));
            }
        }
    }

    #endregion
    #region Command
    public ICommand AddNewExpenseCommand { get; set; }
    public ICommand RefreshExpenseCommand { get; set; }
    public ICommand EditExpenseCommand { get; set; }
    public ICommand DeleteExpenseCommand { get; set; }
    public ICommand RecycleExpenseCommand { get; set; }
    public ICommand MatchWholeWordCommand { get; set; }
    public ICommand MatchCaseCommand { get; set; }
    public ICommand MatchRegexCommand { get; set; }
    public ICommand FindNumberCommand { get; set; }
    public ICommand FindDateCommand { get; set; }
    public ICommand NewExpenseBookCommand { get; set; }
    public ICommand SelectionChangedCommand { get; set; }
    public ICommand ChangedExpenseBookCommand { get; set; }
    public ICommand EditBudgetExBCommand { get; set; } 
    public ICommand TextChangedFilterCommand { get; set; }
    public ICommand FilterCategoryCommand { get; set; } 
    public ICommand ChangedExpenseBudgetCommand { get; set; }
    public ICommand UpdateExpenseBudgetCommand { get; set; }
    public ICommand AddNewCategoryCommand { get; set; }
    public ICommand CategoryDetailCommand { get;set; }
    public ICommand ExpenseBookDetailCommand { get;set; }
    #endregion

    public ExpenseViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _sharedService = serviceProvider.GetRequiredService<SharedService>();

        AddNewExpenseCommand = new NavigateModalCommand<ExpenseAddNewViewModel>(serviceProvider);
        EditExpenseCommand = new NavigateModalCommand<ExpenseEditViewModel>(serviceProvider);
        DeleteExpenseCommand = new NavigateModalCommand<ExpenseDeleteViewModel>(serviceProvider);
        RecycleExpenseCommand = new NavigateModalCommand<ExpenseRecycleViewModel>(serviceProvider);
        NewExpenseBookCommand = new NavigateModalCommand<ExpenseNewExBViewModel>(serviceProvider);
        EditBudgetExBCommand = new NavigateModalCommand<ExpenseEditBudgetViewModel>(serviceProvider);
        AddNewCategoryCommand = new NavigateModalCommand<ExpenseNewCategoryViewModel>(serviceProvider);
        ExpenseBookDetailCommand = new NavigateModalCommand<ExpenseBookDetailViewModel>(serviceProvider);
        CategoryDetailCommand = new NavigateModalCommand<CategoryDetailViewModel>(serviceProvider);

        GetNewest();
        LoadExpenses();
        IsAllChecked = true;
        _sharedService.TriggerAction += LoadNewExpenseBook;
        _expenseStore.TriggerNewCategory += () => LoadCategory();

        RefreshExpenseCommand = new RelayCommand<object>(LoadExpenses);
        //changed selected item exB
        ChangedExpenseBookCommand = new RelayCommand<object>(ExpenseBookChanged);
        //changed selected type filter
        SelectionChangedCommand = new RelayCommand<object>(SelectionChanged);
        //filter
        FindNumberCommand = new RelayCommand<object>(FilterNumber);
        FindDateCommand = new RelayCommand<object>(FilterDate);
        MatchWholeWordCommand = new RelayCommand<object>(Matchwholeword);
        MatchCaseCommand = new RelayCommand<object>(MatchCase);
        MatchRegexCommand = new RelayCommand<object>(MatchRegex);
        //auto
        TextChangedFilterCommand = new RelayCommand<object>(FilterTextChanged);
        FilterCategoryCommand = new RelayCommand<object>(FilterCategory);
        //expesne budget changed
        ChangedExpenseBudgetCommand = new RelayCommand<object>(ChangedExpenseBudget);
        UpdateExpenseBudgetCommand = new RelayCommand<object>(UpdateExpenseBudget);

    }
    public void UpdateExpenseBudget(object? parameter = null) {
        try {
            var budget = DBManager.GetFirst<ExpensesBook>(e => e.UserID == _accountStore.Users.UserID &&
                            e.Month == _expenseStore.ExpenseBook.Month && e.Year == _expenseStore.ExpenseBook.Year);
            if (budget != null) {
                budget.Budget = long.Parse(BudgetOfficial);
                bool check = DBManager.Update(budget);
                if (!check) throw new Exception("Update budget failed");
            }
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    }
    public void ChangedExpenseBudget(object? parameter = null) {
        try {
            BudgetCurrent = string.Empty;
            if (SelectedExpenseBook == null) {
                return;
            }
            if(long.TryParse(BudgetOfficial, out long budgetcur)) {
                foreach (var item in Expensesadvances) {
                    budgetcur -= item.Amount;
                }
                BudgetCurrent = budgetcur.ToString();
                _expenseStore.BudgetCurrentExb = BudgetCurrent;
            }
            else {
                throw new Exception("Can't try to long data");
            }
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        
    }
    public void FilterCategory(object? parameter = null) {
        ReviewCategory();
        FilterTextChanged();
    }
    public void FilterTextChanged(object? parameter = null) {
        try {
            if ((DataFilter == null || DataFilter.Trim() == "") && (CategoryFilter.Count == 0)) {
                LoadExpensesNoneChangedTypeFilter();
                return;
            }
            else if((DataFilter == null || DataFilter.Trim() == "") && (CategoryFilter.Count != 0)) {
                LoadFilterHaveCategory();
            }
            if (DataFilter != "") {
                MatchCase();
            }
        }
        catch (Exception ex) {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public void LoadNewExpenseBook() {
        if (_expenseStore.ExpenseBook != null) {
            HaveExpenseBook = true;
            SourceExpenseBook.Clear();
            var exBitem = DBManager.GetCondition<ExpensesBook>(e => e.UserID == int.Parse(_accountStore.UsersID));
            SourceExpenseBook.Add(new ExpenseBookAdvance());
            foreach (var item in exBitem) {
                SourceExpenseBook.Add(new ExpenseBookAdvance(item.Month, item.Year, item));
            }
            SortObservableCollection(SourceExpenseBook);
            ExpenseBookAdvance exbA = new ExpenseBookAdvance(_expenseStore.ExpenseBook.Month, _expenseStore.ExpenseBook.Year, _expenseStore.ExpenseBook);
            SelectedExpenseBook = exbA;
            TextExpenseBook = exbA.DateExB;
            LoadExpenses();
            ChangedExB();
        }
    }
    public void ChangedExB(object? parameter = null) {
        if (SelectedExpenseBook == null) {
            return;
        }
        BudgetOfficial = SelectedExpenseBook.expensesBook.Budget.ToString();
        var budgetcur = SelectedExpenseBook.expensesBook.Budget;
        foreach(var item in Expensesadvances) {
            budgetcur -= item.Amount;
        }
        BudgetCurrent = budgetcur.ToString();
        _expenseStore.BudgetCurrentExb = BudgetCurrent;
    }
    public void CreateNewExpenseBook() {
        try {
            ExpensesBook exB = new ExpensesBook(DateTime.Now.Month, DateTime.Now.Year, _accountStore.Users, _accountStore.Users.DefaultBudget);
            
            bool check = DBManager.Insert(exB);
            if (!check) throw new Exception("Thêm expense book thất bại");

            if (exB.Month == 1) {
                var categoryPrevious = DBManager.GetCondition<Category>(c => c.UserID == _accountStore.Users.UserID && 12 == c.ExBMonth && exB.Year - 1 == c.ExBYear);
                if (categoryPrevious.Count != 0) {
                    var choose = MessageBox.Show("Bạn có muốn sử dụng category tháng trước không", "Notifcation", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (choose == MessageBoxResult.Yes) {
                        foreach (var item in categoryPrevious) {
                            var cate = new Category(item.Name, exB.Month, exB.Year, _accountStore.Users.UserID);
                            bool check1 = DBManager.Insert(cate);
                            if (!check1) throw new Exception("Thêm category tháng trước thất bại");
                        }
                    }
                }
            }
            else {
                var categoryPrevious = DBManager.GetCondition<Category>(c => c.UserID == _accountStore.Users.UserID && exB.Month - 1 == c.ExBMonth && exB.Year - 1 == c.ExBYear);
                if (categoryPrevious.Count != 0) {
                    var choose = MessageBox.Show("Bạn có muốn sử dụng category tháng trước không", "Notifcation", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (choose == MessageBoxResult.Yes) {
                        foreach (var item in categoryPrevious) {
                            var cate = new Category(item.Name, exB.Month, exB.Year, _accountStore.Users.UserID);
                            bool check1 = DBManager.Insert(cate);
                            if (!check1) throw new Exception("Thêm category tháng trước thất bại");
                        }
                    }
                }
            }
            HaveExpenseBook = true;
            ExpenseBookAdvance ex = new ExpenseBookAdvance(exB.Month, exB.Year, exB);
            SelectedExpenseBook = ex;
            TextExpenseBook = ex.DateExB;
            LoadNewExpenseBook();
        }
        catch (Exception ex) {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public void GetNewest() {
        var items = DBManager.GetCondition<ExpensesBook>(e => int.Parse(_accountStore.UsersID) == e.UserID);
        if(items.Count == 0) {
            HaveExpenseBook = false;
            CreateNewExpenseBook();
            return;
        }
        else {
            HaveExpenseBook = true;
            if(_expenseStore.ExpenseBook == null) {
                ExpensesBook itemmax = items[0];
                ExpensesBook exB = null;
                foreach (var item in items) {
                    if (item.Year > itemmax.Year || (item.Month > itemmax.Month && item.Year == itemmax.Year)) {
                        itemmax = item;
                    }
                    if (item.Year == DateTime.Now.Year && item.Month == DateTime.Now.Month) exB = item;
                }
                if (exB != null) {
                    _expenseStore.ExpenseBook = exB;
                }
                else {
                    _expenseStore.ExpenseBook = itemmax;
                }
            }
        }
    }
    public void ReviewCategory() {
        if (SourceCategory.Count == 0) return;
        if (IsAllChecked == true) {
            CategoryFilter.Clear();
            foreach (var item in SourceCategory) {
                if (item.IsChecked == true && item.Name.CompareTo("All") != 0) {
                    CategoryFilter.Add(item);
                }
            }
            if (CategoryFilter.Count > 0) {
                IsAllChecked = false;
                LoadCategory2();
            }
            else {
                IsAllChecked = true;
                LoadCategory();
            }
        }
        else if(IsAllChecked == false && SourceCategory[0].IsChecked == true) {
            IsAllChecked = true;
            LoadCategory();
            CategoryFilter.Clear();
            foreach (var item in SourceCategory) {
                if (item.IsChecked == true && item.Name.CompareTo("All") != 0) {
                    CategoryFilter.Add(item);
                }
            }
        }
        else {
            CategoryFilter.Clear();
            foreach (var item in SourceCategory) {
                if (item.IsChecked == true && item.Name.CompareTo("All") != 0) {
                    CategoryFilter.Add(item);
                }
            }
        }
        
    }
    private void UpdateItemsCheckState() {
        if (IsAllChecked) {
            foreach (var item in SourceCategory) {
                if (item.Name != "All") {
                    item.ckb.IsChecked = false;
                    item.IsChecked = false;
                }
            }
        }
    }
    public void LoadCategory2(object? parameter = null) {
        var exB = _expenseStore.ExpenseBook;
        var items = DBManager.GetCondition<Category>(c => c.UserID == int.Parse(_accountStore.UsersID) && c.ExBMonth == exB.Month && c.ExBYear == exB.Year);
        if (items == null) return;
        ObservableCollection<CheckBoxCategory> categories = new(SourceCategory);
        SourceCategory.Clear();
        SourceCategory.Add(new CheckBoxCategory(-1, new CheckBox() { Content = "All", IsChecked = false }));
        foreach (var item in categories) {
            if(item.Name != "All") {
                SourceCategory.Add(item);
            }
        }
    }
    public void LoadCategory(object? parameter = null) {
        var exB = _expenseStore.ExpenseBook;
        var items = DBManager.GetCondition<Category>(c => c.UserID == int.Parse(_accountStore.UsersID) && c.ExBMonth == exB.Month && c.ExBYear == exB.Year);
        if (items == null) return;
        SourceCategory.Clear();
        SourceCategory.Add(new CheckBoxCategory(-1, new CheckBox() { Content = "All", IsChecked = true }));
        foreach (var item in items) {
            CheckBox cb = new CheckBox() {
                Content = item.Name.ToString(),
                IsChecked = false
            };
            SourceCategory.Add(new CheckBoxCategory(item.CategoryID, cb));
        }
    }
    public void Filter(string pattern) {
        var userID = int.Parse(_accountStore.UsersID);
        Expensesadvances.Clear();

        List<Expense> items = new List<Expense>();
        if (CategoryFilter.Count == 0) {
            items = DBManager.GetCondition<Expense>(exp => exp.UserID == userID);
        }
        else {
            foreach (var item in CategoryFilter) {
                var i = DBManager.GetCondition<Expense>(e => e.UserID == userID && item.IDCategory == e.CategoryID);
                if (i != null) {
                    foreach(var j in i) {
                        items.Add(j);
                    }
                }
            }
        }
        if (items == null) return;
        if (SelectedTypeFilter.CompareTo("Name") == 0) {
            foreach (var item in items) {
                if (Regex.IsMatch(item.Name, pattern)) {
                    Expensesadvances.Add(new ExpenseAdvance(item));
                }
                else {
                    continue;
                }
            }
        }
        else if (SelectedTypeFilter.CompareTo("Description") == 0) {
            foreach (var item in items) {
                if (item.Description == null) continue;
                if (Regex.IsMatch(item.Description, pattern)) {
                    Expensesadvances.Add(new ExpenseAdvance(item));
                }
                else {
                    continue;
                }
            }
        }
        else if(SelectedTypeFilter.CompareTo("Date") == 0) {
            foreach (var item in items) {
                if (Regex.IsMatch(item.Date.ToString("dd/MM/yyyy"), pattern)) {
                    Expensesadvances.Add(new ExpenseAdvance(item));
                }
                else {
                    continue;
                }
            }
        }
        else if(SelectedTypeFilter.CompareTo("Amount") == 0) {
            foreach (var item in items) {
                if (Regex.IsMatch(item.Amount.ToString(), pattern)) {
                    Expensesadvances.Add(new ExpenseAdvance(item));
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
        ReviewCategory();
        if (DataFilter == null) {
            MessageBox.Show("Dữ liệu có vấn đề! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        string pattern = @"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$";
        
        if (!Regex.IsMatch(DataFilter, pattern)) {
            MessageBox.Show("Nhập dữ liệu theo quy tác DD/MM/yy (ví dụ: 30/04/2024)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var datefilter = DataFilter.Split('/');
        var day = int.Parse(datefilter[0]);
        var month = int.Parse(datefilter[1]);
        var year = int.Parse(datefilter[2]);
        var date = new DateOnly(year, month, day);
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
                    if (day > 29 || day < 1) {
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

        List<Expense> items = new List<Expense>();
        if (CategoryFilter.Count == 0) {
            items = DBManager.GetCondition<Expense>(exp => exp.UserID == userID && exp.Date == date);
        }
        else {
            foreach (var item in CategoryFilter) {
                var i = DBManager.GetCondition<Expense>(e => e.UserID == userID && item.IDCategory == e.CategoryID && e.Date == date);
                if (i != null) {
                    foreach(var j in i) {
                        items.Add(j);
                    }
                };
            }
        }

        if (items == null) return;

        foreach (var item in items) {
            Expensesadvances.Add(new ExpenseAdvance(item));
        }
    }
    public void MatchRegex(object? parameter = null) {
        ReviewCategory();
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
            Regex.Match("", pattern);
            return true;
        }
        catch (ArgumentException) {
            MessageBox.Show("Dữ liệu phải là một regular expression (ví dụ: ^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$)! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
    }
    public void MatchCase(object? parameter = null) {
        ReviewCategory();
        if (DataFilter == null) {
            MessageBox.Show("Dữ liệu có vấn đề! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        string pattern = $@"{Regex.Escape(DataFilter)}";
        Filter(pattern);
    }
    public void Matchwholeword(object? parameter = null) {
        ReviewCategory();
        if (DataFilter == null) {
            MessageBox.Show("Dữ liệu có vấn đề! Vui lòng thử lại.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        string pattern = $@"\b{Regex.Escape(DataFilter)}\b";
        Filter(pattern);
    }
    public void FilterNumber(object? parameter = null) {
        ReviewCategory();
        ReviewCategory();
        string[] patterns = {
        @"^(<)([0-9]+)$",
        @"^(>)([0-9]+)$",
        @"^(=)([0-9]+)$",
        @"^(!)([0-9]+)$",
        @"^(<)(=)([0-9]+)$",
        @"^(>)(=)([0-9]+)$"
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
        List<Expense> items = new();
        try {
            //none category
            if (CategoryFilter.Count == 0) {
                var itemss = DBManager.GetCondition<Expense>(exp => exp.UserID == userID);
                foreach(var item in itemss) {
                    if(condition(item.Amount, number)) {
                        items.Add(item);
                    }
                }
            }
            else {
                //have category
                List<Expense> itemss = new();
                foreach (var item in CategoryFilter) {
                    var i = DBManager.GetCondition<Expense>(e => e.UserID == userID && item.IDCategory == e.CategoryID);
                    if (i != null) {
                        foreach(var j in i) {
                            itemss.Add(j);
                        }
                    }
                }
                foreach (var item in itemss) {
                    if (condition(item.Amount, number)) {
                        items.Add(item);
                    }
                }
            }
            AddExpensesAdvances(items);
        }
        catch (Exception ex) {
            MessageBox.Show("Có lỗi xảy ra", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    }
    private void AddExpensesAdvances(IEnumerable<Expense> items) {
        if (items != null) {
            foreach (var item in items) {
                Expensesadvances.Add(new ExpenseAdvance(item));
            }
        }
    }
    public void SelectionChanged(object? parameter = null) {

        if (SelectedTypeFilter == null) return;
        if(SelectedTypeFilter.CompareTo("Amount") == 0) {
            IsNumber = true;
            IsString = false;
            IsDate = false;
            DataFilter = string.Empty;
        }
        else if(SelectedTypeFilter.CompareTo("Date") == 0){
            IsNumber = false;
            IsString = false;
            IsDate = true;
            DataFilter = string.Empty;
        }
        else {
            IsNumber = false;
            IsString = true;
            IsDate = false;
            DataFilter = string.Empty;
        }
    }
    public void ExpenseBookChanged(object? parameter = null) {
        if(SelectedExpenseBook == null) {
            return;
        }
        else if(SelectedExpenseBook.DateExB.CompareTo("<New>") == 0) {
            return;
        }
        LoadCategory();
        IsDate = true;
        IsNumber = false;
        IsString = false;
        //load data to datagrid
        Expensesadvances.Clear();
        var exB = _expenseStore.ExpenseBook;
        var items = DBManager.GetCondition<Expense>(exp => exp.UserID == int.Parse(_accountStore.UsersID) && exB.Month == exp.ExBMonth && exB.Year == exp.ExBYear);
        foreach (var item in items) {
            Expensesadvances.Add(new ExpenseAdvance(item));
        }
        TextType = "Date";
        SelectedTypeFilter = "Date";
        DataFilter = string.Empty;
        //expensebook
        if (_expenseStore.ExpenseBook != null) {
            ExpenseBookAdvance exbA = new ExpenseBookAdvance(_expenseStore.ExpenseBook.Month, _expenseStore.ExpenseBook.Year, _expenseStore.ExpenseBook);
            SelectedExpenseBook = exbA;
            TextExpenseBook = exbA.DateExB;
        }
        ChangedExB();
    }
    public void LoadFilterHaveCategory() {
        try {
            Expensesadvances.Clear();
            var exB = _expenseStore.ExpenseBook;
            var items = DBManager.GetCondition<Expense>(exp => exp.UserID == int.Parse(_accountStore.UsersID) && exB.Month == exp.ExBMonth && exB.Year == exp.ExBYear);
            HashSet<int> categoryIds = new HashSet<int>(CategoryFilter.Select(c => c.IDCategory));

            List<Expense> filteredExpenses = items.Where(e => categoryIds.Contains(e.CategoryID)).ToList();
            foreach (var item in filteredExpenses) {
                Expensesadvances.Add(new ExpenseAdvance(item));
            }
        }
        catch(Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
    }
    public void LoadExpensesNoneChangedTypeFilter() {
        try {
            //load data to datagrid
            Expensesadvances.Clear();
            var exB = _expenseStore.ExpenseBook;
            var items = DBManager.GetCondition<Expense>(exp => exp.UserID == int.Parse(_accountStore.UsersID) && exB.Month == exp.ExBMonth && exB.Year == exp.ExBYear);
            foreach (var item in items) {
                Expensesadvances.Add(new ExpenseAdvance(item));
            }
        }
        catch(Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    public void LoadExpenses(object? p = null) {
        if (HaveExpenseBook == false) {
            SourceExpenseBook.Clear();
            SourceExpenseBook.Add(new ExpenseBookAdvance());
            //Load data to Type filter
            SourceFilter.Clear();
            //string
            SourceFilter.Add("Date");
            SourceFilter.Add("Name");
            SourceFilter.Add("Description");
            //number
            SourceFilter.Add("Amount");
            TextType = "Date";
            SelectedTypeFilter = "Date";
            DataFilter = string.Empty;
        }
        else {
            HaveExpenseBook = true;
            LoadCategory();
            IsDate = true;
            IsNumber = false;
            IsString = false;
            //load data to datagrid
            Expensesadvances.Clear();
            var exB = _expenseStore.ExpenseBook;
            var items = DBManager.GetCondition<Expense>(exp => exp.UserID == int.Parse(_accountStore.UsersID) && exB.Month == exp.ExBMonth && exB.Year == exp.ExBYear);
            foreach (var item in items) {
                Expensesadvances.Add(new ExpenseAdvance(item));
            }
            //Load data to Type filter
            SourceFilter.Clear();
            //string
            SourceFilter.Add("Date");
            SourceFilter.Add("Name");
            SourceFilter.Add("Description");
            //number
            SourceFilter.Add("Amount");
            TextType = "Date";
            SelectedTypeFilter = "Date";
            DataFilter = string.Empty;

            //expensebook

            SourceExpenseBook.Clear();
            var exBitem = DBManager.GetCondition<ExpensesBook>(e => e.UserID == int.Parse(_accountStore.UsersID));
            SourceExpenseBook.Add(new ExpenseBookAdvance());
            foreach (var item in exBitem) {
                SourceExpenseBook.Add(new ExpenseBookAdvance(item.Month, item.Year, item));
            }
            SortObservableCollection(SourceExpenseBook);
            if (_expenseStore.ExpenseBook != null) {
                ExpenseBookAdvance exbA = new ExpenseBookAdvance(_expenseStore.ExpenseBook.Month, _expenseStore.ExpenseBook.Year, _expenseStore.ExpenseBook);
                SelectedExpenseBook = exbA;
                TextExpenseBook = exbA.DateExB;
            }

            ChangedExB();
        }
    }
    static void SortObservableCollection(ObservableCollection<ExpenseBookAdvance> collection) {
        var sortedList = collection.OrderBy(x => x).ToList();

        collection.Clear();

        foreach (var item in sortedList) {
            collection.Add(item);
        }
    }
    public class CheckBoxCategory {
        public int IDCategory { get; set; }
        public string Name{ get; set; }
        public bool IsChecked { get; set; }
        public CheckBox ckb { get; set; }
        public CheckBoxCategory(int id, CheckBox cb) {
            IDCategory = id;
            ckb = cb;
            Name = cb.Content.ToString();
            IsChecked = (bool)cb.IsChecked;
        }
    }
    public class ExpenseAdvance {
        public Expense exp {  get; set; }
        public int ExpenseID { get; set; }
        public long Amount { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
        public bool Recurring { get; set; }
        public DateTime TimeAdded { get; set; }
        public int CategoryID { get; set; }
        public string Category { get; set; }
        public string FormattedDate => Date.ToString("dd/MM/yyyy");
        public string FormattedAmount => Amount.ToString("N0", new CultureInfo("en-US")).Replace(",", " ");
        public ExpenseAdvance() { }
        public ExpenseAdvance(Expense ex) {
            exp = ex;
            ExpenseID = ex.ExpenseID;
            Amount = ex.Amount;
            Name = ex.Name;
            Description = ex.Description;   
            Date = ex.Date;
            TimeAdded = ex.TimeAdded;
            CategoryID = ex.CategoryID;
            var cate = DBManager.GetFirst<Category>(c => c.CategoryID == ex.CategoryID && c.UserID == ex.UserID);
            Category = cate.Name;
            Recurring = (ex.RecurringExpenseID == null) ? false : true;
        }
    }
    public class ExpenseBookAdvance : IComparable<ExpenseBookAdvance> {
        public ExpenseBookAdvance(int month, int year, ExpensesBook e) {
            DateExB = month.ToString() + "/" + year.ToString();
            expensesBook = e;
            _month = e.Month;
            _year = e.Year;
        }
        public ExpenseBookAdvance() {
            DateExB = "<New>";
        }
        public int _month {  get; set; }
        public int _year {  get; set; }
        public string DateExB { get; set; }
        public ExpensesBook expensesBook { get; set; }
        public int CompareTo(ExpenseBookAdvance other) {
            if (other == null) return 1;

            if (_year == other._year) {
                return _month.CompareTo(other._month);
            }
            return _year.CompareTo(other._year);
        }
    }

}
