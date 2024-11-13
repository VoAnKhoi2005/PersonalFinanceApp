using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.Command;

public class NavigateCommand<TargetViewModel> : BaseCommand where TargetViewModel : BaseViewModel
{
    private readonly NavigationStore _navigationStore;
    private readonly Func<TargetViewModel> _createViewModel;
    private readonly Func<bool> _canExecuteViewModel;

    public NavigateCommand(NavigationStore navigationStore, Func<TargetViewModel> createViewModel, Func<bool>? canExecuteViewModel = null)
    {
        _navigationStore = navigationStore;
        _createViewModel = createViewModel;

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