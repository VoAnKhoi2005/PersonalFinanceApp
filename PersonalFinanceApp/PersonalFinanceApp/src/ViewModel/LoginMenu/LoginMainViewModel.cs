using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class LoginMainViewModel : BaseViewModel
{
    public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

    private readonly NavigationStore _navigationStore;

    public LoginMainViewModel(NavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
    }

    private void OnCurrentViewModelChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}