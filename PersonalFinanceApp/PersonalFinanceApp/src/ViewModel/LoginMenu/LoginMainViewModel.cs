using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class LoginMainViewModel : BaseViewModel
{
    public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

    private readonly LoginNavigationStore _navigationStore;

    public LoginMainViewModel(LoginNavigationStore navigationStore)
    {
        _navigationStore = navigationStore;
    }
}