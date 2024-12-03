using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.ViewModel.Stores;

public class ChartServices
{
    public PlotModel CreateColumnChart(ExpensesBook expensesBook)
    {
        if (expensesBook == null)
            throw new Exception("Expense book can not be null");

        var expenseChart = new PlotModel
        {
            TextColor = OxyColors.White,
            Title = $"{expensesBook.Month}/{expensesBook.Year}"
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
        for (int day = 1; day <= DateTime.DaysInMonth(expensesBook.Year, expensesBook.Month); day++)
        {
            long curSum = expensesBook.Expenses.Where(ex => ex.Date == new DateOnly(expensesBook.Year, expensesBook.Month, day)).Sum(ex => ex.Amount);
            barSeries.Items.Add(new BarItem(curSum));
        }

        expenseChart.Series.Add(barSeries);
        return expenseChart;
    }
}