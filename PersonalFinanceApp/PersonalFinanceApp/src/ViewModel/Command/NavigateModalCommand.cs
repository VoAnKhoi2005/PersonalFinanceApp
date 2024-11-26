using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.Command;

public class NavigateModalCommand<TViewModel> : BaseCommand where TViewModel : BaseViewModel
{
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly Func<TViewModel> _createViewModel;
    private readonly Func<bool> _canExecuteViewModel;

    public NavigateModalCommand(IServiceProvider serviceProvider, Func<bool>? canExecuteViewModel = null)
    {
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _createViewModel = serviceProvider.GetRequiredService<TViewModel>;
        _canExecuteViewModel = canExecuteViewModel ?? (() => true);
    }

    public override void Execute(object parameter)
    {
        //_modalNavigationStore.CurrentModalViewModel = _createViewModel()
        _modalNavigationStore.Navigate(_createViewModel());
    }

    public override bool CanExecute(object parameter) => _canExecuteViewModel();
}