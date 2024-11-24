using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class DashboardViewModel : BaseViewModel
{
    public ICommand testCommand { get; set; }

    public DashboardViewModel(IServiceProvider serviceProvider)
    {

    }
}