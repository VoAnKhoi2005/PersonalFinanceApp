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

        var columnChart = new PlotModel
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

        columnChart.Axes.Add(categoryAxis);
        columnChart.Axes.Add(valueAxis);

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

        columnChart.Series.Add(barSeries);
        return columnChart;
    }

    public PlotModel CreatePieChart(ExpensesBook expensesBook)
    {
        if (expensesBook == null)
            throw new Exception("Expense book can not be null");

        var pieChart = new PlotModel
        {
            TextColor = OxyColors.White
        };

        PieSeries pieSeries = new PieSeries
        {
            StrokeThickness = 1.0,
            InsideLabelPosition = 0.8,
            InsideLabelFormat = "{1:0.0}%",
            AngleSpan = 360,
            StartAngle = 0
        };


        long totalExpense = expensesBook.Expenses.Sum(ex => ex.Amount);
        foreach (var category in expensesBook.Categories)
        {
            double percentage = category.Expenses.Sum(ex => ex.Amount) / totalExpense * 100.0;
            pieSeries.Slices.Add(new PieSlice(category.Name, percentage)
            {
                IsExploded = false
            });
        }

        pieChart.Series.Add(pieSeries);
        return pieChart;
    }
}