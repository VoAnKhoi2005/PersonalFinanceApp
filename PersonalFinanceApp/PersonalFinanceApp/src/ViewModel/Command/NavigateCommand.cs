using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.Command;

public class NavigateCommand<TViewModel> : BaseCommand where TViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<object?, TViewModel> _createViewModel;
    private readonly Func<bool> _canExecuteViewModel;

    public NavigateCommand(IServiceProvider serviceProvider,
                           Func<object?, TViewModel>? createViewModel = null, 
                           Func<bool>? canExecuteViewModel = null)
    {
        _navigationStore = serviceProvider.GetRequiredService<NavigationStore>();
        _createViewModel = createViewModel ?? (_ => serviceProvider.GetRequiredService<TViewModel>());
        _canExecuteViewModel = canExecuteViewModel ?? (() => true);
    }

    public override void Execute(object parameter)
    {
        _navigationStore.CurrentViewModel = _createViewModel(parameter);
    }

    public override bool CanExecute(object parameter) => _canExecuteViewModel();
}