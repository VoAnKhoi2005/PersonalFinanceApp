namespace PersonalFinanceApp.ViewModel.Stores;

public class ModalNavigationStore
{
    public event Action? CurrentModalViewModelChanged;

    private BaseViewModel? _currentModalViewModel;
    public BaseViewModel? CurrentModalViewModel
    {
        get => _currentModalViewModel;
        set
        {
            _currentModalViewModel = value;
            OnCurrentModalViewModelChanged();
        }
    }

    public bool IsOpen => _currentModalViewModel != null;

    public void Close()
    {
        CurrentModalViewModel = null;
    }

    private void OnCurrentModalViewModelChanged()
    {
        CurrentModalViewModelChanged?.Invoke();
    }
}