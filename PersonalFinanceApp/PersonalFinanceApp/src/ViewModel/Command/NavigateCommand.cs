using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.Command;

public class NavigateCommand<TViewModel> : BaseCommand where TViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<TViewModel> _createViewModel;
    private readonly Func<bool> _canExecuteViewModel;

    public NavigateCommand(IServiceProvider serviceProvider, Func<bool>? canExecuteViewModel = null)
    {
        _navigationStore = serviceProvider.GetRequiredService<NavigationStore>();
        _createViewModel = () => serviceProvider.GetRequiredService<TViewModel>();
        if (canExecuteViewModel == null)
            _canExecuteViewModel = () => true;
        else
            _canExecuteViewModel = canExecuteViewModel;
    }

    public override void Execute(object parameter)
    {
        _navigationStore.CurrentViewModel = _createViewModel();
    }

    public override bool CanExecute(object parameter) => _canExecuteViewModel();
}