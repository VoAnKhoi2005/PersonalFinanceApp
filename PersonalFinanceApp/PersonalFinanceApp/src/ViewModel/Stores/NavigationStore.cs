namespace PersonalFinanceApp.ViewModel.Stores;

public class NavigationStore
{
    public event Action CurrentViewModelChanged;

    private BaseViewModel _currentViewModel;
    public BaseViewModel CurrentViewModel
    {
        get => _currentViewModel;
        set
        {
            _currentViewModel = value;
            OnCurrentViewModelChanged();
        }
    }

    private void OnCurrentViewModelChanged()
    {
        CurrentViewModelChanged?.Invoke();
    }
}