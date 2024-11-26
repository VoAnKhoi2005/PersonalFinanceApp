namespace PersonalFinanceApp.ViewModel.Stores;

public class ModalNavigationStore
{
    public event Action? CurrentModalViewModelChanged;
    private List<BaseViewModel> _viewModels = new List<BaseViewModel>();

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

    public void Navigate(BaseViewModel viewModel)
    {
        CurrentModalViewModel = viewModel;
        _viewModels.Add(viewModel);
    }

    public void Close()
    {
        _viewModels.RemoveAt(_viewModels.Count - 1);
        if (_viewModels.Count > 0) 
            CurrentModalViewModel = _viewModels.Last();
        else 
            CurrentModalViewModel = null;
    }

    private void OnCurrentModalViewModelChanged()
    {
        CurrentModalViewModelChanged?.Invoke();
    }
}