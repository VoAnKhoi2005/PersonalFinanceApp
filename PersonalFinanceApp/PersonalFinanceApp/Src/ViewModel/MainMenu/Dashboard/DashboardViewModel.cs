using System.Windows.Input;
using OxyPlot;
using PersonalFinanceApp.Model;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class DashboardViewModel : BaseViewModel
{
    private readonly ChartServices _chartServices;
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
    public bool HasNoData => ExpenseChart == null;

    public ICommand ColumnChartNavigateCommand { get; set; }

    public DashboardViewModel(IServiceProvider serviceProvider)
    {
        ColumnChartNavigateCommand = new NavigateCommand<SummaryViewModel>(serviceProvider);

        _chartServices = serviceProvider.GetRequiredService<ChartServices>();
        CustomPlotController = new PlotController();
        CustomPlotController.UnbindMouseDown(OxyMouseButton.Left);
        CustomPlotController.BindMouseEnter(PlotCommands.HoverSnapTrack);

        ExpensesBook? curExpensesBook = DBManager.GetFirst<ExpensesBook>(exB => exB.Year == DateTime.Now.Year && 
                                                                                exB.Month == DateTime.Now.Month);

        if (curExpensesBook != null)
            ExpenseChart = _chartServices.CreateColumnChart(curExpensesBook);
        else
            ExpenseChart = null;
    }
}