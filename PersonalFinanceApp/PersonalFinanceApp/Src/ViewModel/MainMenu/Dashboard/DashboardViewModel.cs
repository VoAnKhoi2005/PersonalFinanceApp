using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using PersonalFinanceApp.Model;
using SkiaSharp;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class DashboardViewModel : BaseViewModel
{
    public List<PieSeries<double>> BudgetSeries { get; set; }

    public List<ColumnSeries<DateTimePoint>> ActivitySeries { get; set; }
    public List<ICartesianAxis> XAxisActivity { get; set; }
    public List<ICartesianAxis> YAxisActivity { get; set; }
    public bool HasNoActivityData { get; set; }

    public DashboardViewModel(IServiceProvider serviceProvider)
    {
        BudgetSeries = CreateDoughnutChartRandom();

        //XAxisActivity = new List<ICartesianAxis>
        //{
        //    new DateTimeAxis(TimeSpan.FromDays(1), date => date.ToString("dd"))
        //};
        XAxisActivity = new List<ICartesianAxis>
        {
            new DateTimeAxis(TimeSpan.FromDays(1), date => date.ToString("dd"))
            {
                LabelsPaint = new SolidColorPaint(SKColors.White),
                TextSize = 13,
                Padding = new Padding(0, 0, 0, 10),
                SeparatorsPaint = null,
                MinStep = TimeSpan.FromDays(1).Ticks,
                ForceStepToMin = true,
            }
        };
        YAxisActivity = new List<ICartesianAxis>
        {
            new Axis
            {
                LabelsPaint = new SolidColorPaint(SKColors.White),
            }
        };
        ActivitySeries = CreateActivityChartRandom();
    }

    public List<PieSeries<double>> CreateDoughnutChart(ExpensesBook expensesBook)
    {
        ExpensesBook ExBTemp = expensesBook;
        long remainBudget = ExBTemp.Budget - ExBTemp.Expenses.Sum(ex => ex.Amount);
        ExBTemp.Categories.Add(new Category
        {
            Name = "Remaining budget",
            Expenses = new List<Expense> { new Expense { Amount = remainBudget } }
        });

        var pieSeries = new List<PieSeries<double>>();
        foreach (Category category in ExBTemp.Categories)
        {
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

    public List<PieSeries<double>> CreateDoughnutChartRandom()
    {
        var random = new Random();
        var pieSeries = new List<PieSeries<double>>();
        for (int i = 0; i < 5; i++)
        {
            var pieSerie = new PieSeries<double>
            {
                Values = new List<double> { random.Next(1000000, 1000000000) },
                Name = $"Category {i}",
                InnerRadius = 0.6,
                MaxRadialColumnWidth = 30,
                ToolTipLabelFormatter = point => point.Model.ToString("N0") + " VND",
            };
            pieSeries.Add(pieSerie);
        }

        return pieSeries;
    }

    public List<ColumnSeries<DateTimePoint>> CreateActivityChart(ExpensesBook expensesBook)
    {
        List<DateTimePoint> dateTimePoints = new List<DateTimePoint>();
        for (int i = 1; i <= DateTime.DaysInMonth(expensesBook.Year, expensesBook.Month); i++)
        {
            DateTimePoint dateTimePoint = new DateTimePoint
            {
                DateTime = new DateTime(expensesBook.Year, expensesBook.Month, i),
                Value = expensesBook.Expenses.Where(ex => ex.Date.Day == i).Sum(ex => ex.Amount),
            };
            dateTimePoints.Add(dateTimePoint);
        }

        List<ColumnSeries<DateTimePoint>> columnSeries = new List<ColumnSeries<DateTimePoint>>
        {
            new ColumnSeries<DateTimePoint>
            {
                Values = dateTimePoints,
                DataLabelsFormatter = _ => "",
            }
        };
        return columnSeries;
    }

    public List<ColumnSeries<DateTimePoint>> CreateActivityChartRandom()
    {
        Random random = new Random();
        List<DateTimePoint> dateTimePoints = new List<DateTimePoint>();
        for (int i = 1; i <= 30; i++)
        {
            dateTimePoints.Add(new DateTimePoint(new DateTime(2024, 1, i), random.Next(10000, 10000000)));
        }

        List<ColumnSeries<DateTimePoint>> columnSeries = new List<ColumnSeries<DateTimePoint>>
        {
            new ColumnSeries<DateTimePoint>
            {
                Values = dateTimePoints,
                DataLabelsFormatter = _ => "",
                XToolTipLabelFormatter = point => point.Model.DateTime.ToString("dd/MM/yyyy"),
                YToolTipLabelFormatter = point => point.Model.Value.HasValue ? point.Model.Value.Value.ToString("N0") + " VND" : string.Empty,
                Fill = new SolidColorPaint(SKColors.DodgerBlue)
            }
        };

        return columnSeries;
    }
}