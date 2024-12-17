using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using PersonalFinanceApp.Model;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.ViewModel.Command;
using System.Collections.ObjectModel;
using PersonalFinanceApp.Src.ViewModel.Stores;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Input;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class DashboardViewModel : BaseViewModel
{
    private readonly ExpenseStore _expenseStore;
    private readonly AccountStore _accountStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly SharedService _sharedService;
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
    public bool HasNoData => false;

    #endregion
    #region Command
    public ICommand ExpenseNavigateCommand { get; set; }
    public ICommand ChangedExpenseBookCommand { get; set; }
    #endregion
    private List<PieSeries<double>> _budgetSeries;
    public List<PieSeries<double>> BudgetSeries {
        get => _budgetSeries;
        set {
            _budgetSeries = value;
            OnPropertyChanged(nameof(BudgetSeries));
        }
    }

    //Them vao
    public bool HasNoActivityData => ActivityPlotModel == null;
    private PlotModel? _activityPlotModel;
    public PlotModel? ActivityPlotModel
    {
        get => _activityPlotModel;
        set
        {
            _activityPlotModel = value;
            OnPropertyChanged();
        }
    }

    private PlotController _customPlotController;
    public PlotController CustomPlotController
    {
        get => _customPlotController;
        set
        {
            _customPlotController = value;
            OnPropertyChanged();
        }
    }

    public DashboardViewModel(IServiceProvider serviceProvider)
    {
        ExpenseNavigateCommand = new NavigateCommand<ExpenseViewModel>(serviceProvider);

        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _expenseStore = serviceProvider.GetRequiredService<ExpenseStore>();
        GoalViewModels = new ObservableCollection<DashboardGoalViewModel>();
        _sharedService = serviceProvider.GetRequiredService<SharedService>();

        usr = int.Parse(_accountStore.UsersID);

        BudgetSeries = new List<PieSeries<double>>();

        LoadDashBoard();

        ChangedExpenseBookCommand = new RelayCommand<object>(ExpenseBookChanged);

        _sharedService.Notify();


        //them vao
        CustomPlotController = new PlotController();
        CustomPlotController.UnbindMouseDown(OxyMouseButton.Left);
        CustomPlotController.BindMouseEnter(PlotCommands.HoverSnapTrack);

        //ActivityPlotModel = CreateActivityPlotModel(null);
    }

    //them vao
    public PlotModel CreateActivityPlotModel(ExpensesBook exB)
    {
        if (exB == null)
            return null;

        PlotModel plotModel = new PlotModel();

        var categoryAxis = new CategoryAxis
        {
            Title = "Days",
            Position = OxyPlot.Axes.AxisPosition.Bottom,
            TextColor = OxyColors.White,
            TitleColor = OxyColors.White,
            Key = "DaysAxis",
        };
        for (int day = 1; day <= DateTime.DaysInMonth(exB.Year, exB.Month); day++)
        {
            categoryAxis.Labels.Add(day.ToString());
        }
        plotModel.Axes.Add(categoryAxis);

        var valueAxis = new LinearAxis
        {
            Position = OxyPlot.Axes.AxisPosition.Left,
            Minimum = 0,
            TextColor = OxyColors.White,
            TitleColor = OxyColors.White,
            LabelFormatter = CustomCurrencyFormat,
            MajorGridlineStyle = LineStyle.Solid,
            Key = "ValueAxis",
        };
        plotModel.Axes.Add(valueAxis);

        List<BarItem> expensesDaily = new List<BarItem>();
        for (int day = 1; day <= DateTime.DaysInMonth(exB.Year, exB.Month); day++)
        {
            expensesDaily.Add(new BarItem
            {
                Value = exB.Expenses.Where(ex => ex.Date.Day == day && ex.UserID == exB.UserID).Sum(ex => ex.Amount),

            });
        }

        BarSeries columnSeries = new BarSeries
        {
            LabelPlacement = LabelPlacement.Outside,
            TextColor = OxyColors.White,
            ItemsSource = expensesDaily,
            XAxisKey = "ValueAxis",
            YAxisKey = "DaysAxis",
            FillColor = OxyColors.DodgerBlue,
            TrackerFormatString = "{2:N0} VND",
        };
        plotModel.Series.Add(columnSeries);
        return plotModel;
    }

    private string CustomCurrencyFormat(double value)
    {
        if (value >= 1000000)
            return (value / 1000000.0).ToString("F1") + "M";
        if (value >= 100000)
            return (value / 1000).ToString("F1") + "K";
        return value.ToString("N0");
    }
    //.///////////////////////////////////////////////////////////////////


    public void UpdatePieChart(ExpensesBook expensesBook) {
        var newSeries = CreateDoughnutChart(expensesBook);
        BudgetSeries = newSeries;
    }
//    public void UpdateColumnChart(ExpensesBook expensesBook) {
//        XAxisActivity = new List<ICartesianAxis>
//{
//            new DateTimeAxis(TimeSpan.FromDays(1), date => date.ToString("dd"))
//            {
//                LabelsPaint = new SolidColorPaint(SKColors.White),
//                TextSize = 13,
//                Padding = new Padding(0, 0, 0, 10),
//                SeparatorsPaint = null,
//                MinStep = TimeSpan.FromDays(1).Ticks,
//                ForceStepToMin = true,
//            }
//        };

//        YAxisActivity = new List<ICartesianAxis>
//        {
//            new Axis
//            {
//                LabelsPaint = new SolidColorPaint(SKColors.White),
//            }
//        };
//        var newSeries = CreateActivityChart(expensesBook);
//        ActivitySeries = newSeries;
//    }
    public List<PieSeries<double>> CreateDoughnutChart(ExpensesBook expensesBook)
    {
        ExpensesBook ExBTemp = expensesBook;
        var items = DBManager.GetCondition<Expense>(e => e.UserID == expensesBook.UserID && e.ExBMonth == expensesBook.Month && e.ExBYear == expensesBook.Year);
        ExBTemp.Expenses = items;
        long remainBudget = ExBTemp.Budget - ExBTemp.Expenses.Sum(ex => ex.Amount);

        var listcate = DBManager.GetCondition<Category>(c => c.UserID == expensesBook.UserID && c.ExBMonth == expensesBook.Month && c.ExBYear == expensesBook.Year);

        ExBTemp.Categories = listcate;

        ExBTemp.Categories.Add(new Category
        {
            Name = "Remaining budget",
            Expenses = new List<Expense> { new Expense { Amount = remainBudget } }
        });

        var pieSeries = new List<PieSeries<double>>();
        foreach (Category category in ExBTemp.Categories)
        {
            var exp = DBManager.GetCondition<Expense>(e => e.UserID == expensesBook.UserID && e.CategoryID == category.CategoryID && 
                            e.ExBMonth == category.ExBMonth && e.ExBYear == category.ExBYear);
            if (exp.Count != 0) category.Expenses = exp;
            var pieSerie = new PieSeries<double>
            {
                Values = new List<double> { category.Expenses.Sum(ex => ex.Amount) },
                Name = category.Name,
                InnerRadius = 0.6,
                MaxRadialColumnWidth = 60,
                DataLabelsPosition = PolarLabelsPosition.Outer,
                ToolTipLabelFormatter = point => point.Model.ToString("N0") + " VND",
            };
            pieSeries.Add(pieSerie);
        }

        return pieSeries;
    }
    
    public void LoadGoal() {
        //load goal
        GoalViewModels.Clear();
        List<Goal> goals = DBManager.GetCondition<Goal>(g => g.User.UserID == usr);
        foreach (var goal in goals) {
            GoalViewModels.Add(new DashboardGoalViewModel(_serviceProvider, goal));
        }
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


        UpdatePieChart(SelectedExpenseBook.expensesBook);
        //UpdateColumnChart(SelectedExpenseBook.expensesBook);
        LoadTotal();
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
            BudgetSeries = CreateDoughnutChart(itemmax);

            ActivityPlotModel = CreateActivityPlotModel(itemmax);
        }
    }
    public void LoadDashBoard() {
        if (DBManager.GetFirst<ExpensesBook>(e => e.UserID == usr) == null) return;
        GetNewest();
        LoadSourceExpeseBook();
        LoadTotal();
        LoadGoal();
    }
    public decimal BudgetRemainExpenseBook(ExpensesBook exb) {
        decimal sum = 0;
        var items = DBManager.GetCondition<Expense>(e => e.UserID == usr && e.ExBMonth == exb.Month && e.ExBYear == exb.Year);
        foreach(var item in items) {
            sum += item.Amount;
        }
        return exb.Budget - sum;
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