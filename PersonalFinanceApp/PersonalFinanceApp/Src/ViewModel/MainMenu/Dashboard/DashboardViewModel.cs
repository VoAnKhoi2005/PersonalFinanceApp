using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PersonalFinanceApp.Model;
using System.Globalization;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class DashboardViewModel : BaseViewModel
{
    private PlotModel? _expenseChart;
    public PlotModel? ExpenseChart
    {
        get => _expenseChart;
        set
        {
            _expenseChart = value;
            OnPropertyChanged();
        }
    }

    public PlotController CustomPlotController { get; set; }

    public DashboardViewModel(IServiceProvider serviceProvider)
    {
        CustomPlotController = new PlotController();
        CustomPlotController.UnbindMouseDown(OxyMouseButton.Left);
        CustomPlotController.BindMouseEnter(PlotCommands.HoverSnapTrack);

        ExpenseChart = CreateChart(null);
    }

    private PlotModel CreateChart(ExpensesBook expensesBook)
    {
        var expenseChart = new PlotModel
        {
            TextColor = OxyColors.White,
            Title = "11/2024"
        };

        var categoryAxis = new CategoryAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Day",
            Key = "y"
        };

        var valueAxis = new LinearAxis
        {
            Position = AxisPosition.Left,
            StringFormat = "#,0",
            Minimum = 0,
            Key = "x"
        };

        expenseChart.Axes.Add(categoryAxis);
        expenseChart.Axes.Add(valueAxis);

        var barSeries = new BarSeries
        {
            Title = "Daily Expenses",
            FillColor = OxyColors.SteelBlue,
            LabelPlacement = LabelPlacement.Outside,
            TrackerFormatString = "{Value:#,0}",
            XAxisKey = "x",
            YAxisKey = "y"
        };
        var random = new Random();
        for (int day = 1; day <= 30; day++)
        {
            int rnd = random.Next(10000, 10000000);
            long curSum = 0;
            if (expensesBook != null)
                curSum = expensesBook.Expenses.Where(ex => ex.Date == new DateOnly(expensesBook.Year, expensesBook.Month, day)).Sum(ex => ex.Amount);
            barSeries.Items.Add(new BarItem(curSum));
        }

        expenseChart.Series.Add(barSeries);
        return expenseChart;
    }
}