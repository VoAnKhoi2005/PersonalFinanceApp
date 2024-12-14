using System.Windows.Input;
using OxyPlot;
using PersonalFinanceApp.Model;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Collections.ObjectModel;
using PersonalFinanceApp.Src.ViewModel.Stores;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class DashboardViewModel : BaseViewModel
{
    private readonly ChartServices _chartServices;
    private readonly ExpenseStore _expenseStore;
    private readonly AccountStore _accountStore;
    private readonly IServiceProvider _serviceProvider;
    #region Properties
    public int usr { get; set; }
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
    //budget left
    public string BudgetLeft {
        get => _budgetLeft;
        set {
            if (_budgetLeft != value) {
                _budgetLeft = value;
                OnPropertyChanged();
            }
        }
    }
    private string _budgetLeft;
    //expense
    public string ExpenseTotal {
        get => _expenseTotal;
        set {
            if (_expenseTotal != value) {
                _expenseTotal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _expenseTotal;
    //saving
    public string SavingTotal {
        get => _savingTotal;
        set {
            if (_savingTotal != value) {
                _savingTotal = value;
                OnPropertyChanged();
            }
        }
    }
    private string _savingTotal;
    //EXPENSE BOOK
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
                if (value != null) {
                    _expenseStore.ExpenseBook = value.expensesBook;
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

    //source category
    public ObservableCollection<TextBlock> SourceCategory {
        get => _sourceCategory;
        set {
            if (_sourceCategory != value) {
                _sourceCategory = value;
                OnPropertyChanged();
            }
        }
    }
    private ObservableCollection<TextBlock> _sourceCategory = new();

    //source goal
    public ObservableCollection<DashboardGoalViewModel> GoalViewModels {
        get => _goalViewModels;
        set {
            if (_goalViewModels != value) {
                _goalViewModels.CollectionChanged -= OnGoalplanCardViewModelsChanged;
                _goalViewModels = value;
                _goalViewModels.CollectionChanged += OnGoalplanCardViewModelsChanged;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasNoGoal));
            }
        }
    }
    private ObservableCollection<DashboardGoalViewModel> _goalViewModels = new ObservableCollection<DashboardGoalViewModel>();
    private void OnGoalplanCardViewModelsChanged(object? sender, NotifyCollectionChangedEventArgs e) {
        OnPropertyChanged(nameof(HasNoGoal));
    }

    public bool HasNoGoal => !GoalViewModels.Any();
    //expense
    public PlotModel? ExpenseChart
    {
        get => _expenseChart;
        set
        {
            _expenseChart = value;
            OnPropertyChanged();
        }
    }
    private PlotModel? _expenseChart;
    //budget
    public PlotModel? BudgetChart {
        get => _budgetChart;
        set {
            _budgetChart = value;
            OnPropertyChanged();
        }
    }
    private PlotModel? _budgetChart;

    public PlotController CustomPlotController { get; set; }
    public PlotController CustomPlotController2 { get; set; }
    public bool HasNoData => ExpenseChart == null;
    #endregion
    #region Command
    public ICommand ExpenseNavigateCommand { get; set; }
    public ICommand ChangedExpenseBookCommand { get; set; }
    public ICommand GoalNavigateCommand { get; set; }
    #endregion
    public DashboardViewModel(IServiceProvider serviceProvider)
    {
        ExpenseNavigateCommand = new NavigateCommand<ExpenseViewModel>(serviceProvider);
        GoalNavigateCommand = new NavigateCommand<GoalplanViewModel>(serviceProvider);

        _serviceProvider = serviceProvider;
        _chartServices = serviceProvider.GetRequiredService<ChartServices>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        GoalViewModels = new ObservableCollection<DashboardGoalViewModel>();

        usr = int.Parse(_accountStore.UsersID);

        LoadDashBoard();

        ChangedExpenseBookCommand = new RelayCommand<object>(ExpenseBookChanged);
    }
    public void LoadGoal() {
        //load goal
        GoalViewModels.Clear();
        List<Goal> goals = DBManager.GetCondition<Goal>(g => g.User.UserID == usr);
        foreach (var goal in goals) {
            GoalViewModels.Add(new DashboardGoalViewModel(_serviceProvider, goal));
        }
    }
    public void LoadBudget() {
        //load budget
        SourceCategory.Clear();

        ExpensesBook? curExpensesBook = DBManager.GetFirst<ExpensesBook>(exB => exB.Year == SelectedExpenseBook._year &&
                                                                                exB.Month == SelectedExpenseBook._month && exB.UserID == int.Parse(_accountStore.UsersID));
        var listCategory = DBManager.GetCondition<Category>(c => c.UserID == usr && c.ExBMonth == SelectedExpenseBook._month && c.ExBYear == SelectedExpenseBook._year);
        foreach (var category in listCategory) {
            var listExpense = DBManager.GetCondition<Expense>(e => e.ExBMonth == SelectedExpenseBook._month &&
                                                               e.ExBYear == SelectedExpenseBook._year &&
                                                               e.UserID == usr && e.CategoryID == category.CategoryID);
            category.Expenses = listExpense;

            TextBlock tb = new TextBlock() {
                Text = category.Name
            };
            SourceCategory.Add(tb);
        }
        var expenses = DBManager.GetCondition<Expense>(e => e.ExBMonth == SelectedExpenseBook._month &&
                                                                e.ExBYear == SelectedExpenseBook._year &&
                                                                e.UserID == usr);
        if (curExpensesBook != null) {
            curExpensesBook.Categories = listCategory;
            curExpensesBook.Expenses = expenses;
            BudgetChart = _chartServices.CreatePieChart(curExpensesBook);
        }
        else
            BudgetChart = null;
    }
    public void LoadTotal() {
        //load expense
        var exp = DBManager.GetCondition<Expense>(e => e.UserID == usr && _expenseStore.ExpenseBook.Month == e.ExBMonth && _expenseStore.ExpenseBook.Year == e.ExBYear);
        long sum = 0;
        foreach (var item in exp) {
            sum += item.Amount;
        }
        ExpenseTotal = sum.ToString();

        //load budget left
        BudgetLeft = (_expenseStore.ExpenseBook.Budget - sum).ToString();

        //saving
        decimal saving = 0;
        foreach (var item in SourceExpenseBook) {
            saving += decimal.Parse(item.BudgetRemain);
        }
        SavingTotal = saving.ToString();
    }
    public void LoadSourceExpeseBook() {
        //load exB
        SourceExpenseBook.Clear();
        var exBitem = DBManager.GetCondition<ExpensesBook>(e => e.UserID == usr);
        foreach (var item in exBitem) {
            var t = new ExpenseBookAdvance(item);
            t.BudgetRemain = BudgetRemainExpenseBook(item).ToString();
            SourceExpenseBook.Add(t);
        }
        SortObservableCollection(SourceExpenseBook);

        if (_expenseStore.ExpenseBook != null) {
            ExpenseBookAdvance exbA = new ExpenseBookAdvance(_expenseStore.ExpenseBook);
            SelectedExpenseBook = exbA;
            TextExpenseBook = exbA.DateExB;
        }
    }
    public void ExpenseBookChanged(object? parameter = null) {
        if (SelectedExpenseBook == null) {
            return;
        }
        //expensebook
        if (_expenseStore.ExpenseBook != null) {
            ExpenseBookAdvance exbA = new ExpenseBookAdvance(_expenseStore.ExpenseBook);
            SelectedExpenseBook = exbA;
            TextExpenseBook = exbA.DateExB;
        }
        LoadTotal();
        LoadBudget();
        LoadColumnChart();
    }
    public void GetNewest() {
        var items = DBManager.GetCondition<ExpensesBook>(e => usr == e.UserID);
        if (items.Count == 0) {
            HaveExpenseBook = false;
            return;
        }
        else {
            HaveExpenseBook = true;
            ExpensesBook itemmax = items[0];
            foreach (var item in items) {
                if (item.Year > itemmax.Year || (item.Month > itemmax.Month && item.Year == itemmax.Year)) {
                    itemmax = item;
                }
            }
            _expenseStore.ExpenseBook = itemmax;
        }
    }
    public void LoadDashBoard() {

        CustomPlotController = new PlotController();
        CustomPlotController.UnbindMouseDown(OxyMouseButton.Left);
        CustomPlotController.BindMouseEnter(PlotCommands.HoverSnapTrack);
        if (DBManager.GetFirst<ExpensesBook>(e => e.UserID == usr) == null) return;
        GetNewest();
        LoadSourceExpeseBook();
        LoadTotal();
        LoadGoal();
        LoadBudget();
        LoadColumnChart();
    }
    public decimal BudgetRemainExpenseBook(ExpensesBook exb) {
        decimal sum = 0;
        var items = DBManager.GetCondition<Expense>(e => e.UserID == usr && e.ExBMonth == exb.Month && e.ExBYear == exb.Year);
        foreach(var item in items) {
            sum += item.Amount;
        }
        return exb.Budget - sum;
    }
    public void LoadColumnChart() {
        ExpensesBook? curExpensesBook = DBManager.GetFirst<ExpensesBook>(exB => exB.UserID == usr && exB.Month == SelectedExpenseBook._month && exB.Year == SelectedExpenseBook._year);
        var expenses = DBManager.GetCondition<Expense>(e => e.UserID == usr && e.ExBMonth == SelectedExpenseBook._month && e.ExBYear == SelectedExpenseBook._year);
        if (curExpensesBook != null) {
            curExpensesBook.Expenses = expenses;
            ExpenseChart = _chartServices.CreateColumnChart(curExpensesBook);
        }
        else
            ExpenseChart = null;
    }
    static void SortObservableCollection(ObservableCollection<ExpenseBookAdvance> collection) {
        var sortedList = collection.OrderBy(x => x).ToList();

        collection.Clear();

        foreach (var item in sortedList) {
            collection.Add(item);
        }
    }
    public class ExpenseBookAdvance : IComparable<ExpenseBookAdvance> {
        public ExpenseBookAdvance(ExpensesBook e) {
            DateExB = e.Month.ToString() + "/" + e.Year.ToString();
            expensesBook = e;
            _month = e.Month;
            _year = e.Year;
        }
        public int _month { get; set; }
        public int _year { get; set; }
        public string DateExB { get; set; }
        public string BudgetRemain { get; set; }
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