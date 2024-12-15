using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.ViewModel.Stores;

public class ChartServices
{
    public List<PieSeries<double>> CreateDoughnutChart(ExpensesBook expensesBook)
    {
        ExpensesBook ExBTemp = expensesBook;
        long remainBudget = ExBTemp.Budget - ExBTemp.Expenses.Sum(ex => ex.Amount);
        ExBTemp.Categories.Add(new Category
        {
            Name = "Remaining budget",
            Expenses = new List<Expense> {new Expense {Amount = remainBudget}}
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
                ToolTipLabelFormatter =  point => point.Model.ToString("N0") + " VND",
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
                ToolTipLabelFormatter =  point => point.Model.ToString("N0") + " VND",
            };
            pieSeries.Add(pieSerie);
        }

        return pieSeries;
    }
}