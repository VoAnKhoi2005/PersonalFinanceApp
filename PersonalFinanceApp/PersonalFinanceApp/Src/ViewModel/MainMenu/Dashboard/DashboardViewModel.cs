using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Stores;
using SkiaSharp;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class DashboardViewModel : BaseViewModel
{
    public List<PieSeries<double>> BudgetSeries { get; set; }
    private readonly ChartServices _chartServices;

    public DashboardViewModel(IServiceProvider serviceProvider)
    {
        _chartServices = serviceProvider.GetRequiredService<ChartServices>();
        BudgetSeries = _chartServices.CreateDoughnutChartRandom();
    }
}