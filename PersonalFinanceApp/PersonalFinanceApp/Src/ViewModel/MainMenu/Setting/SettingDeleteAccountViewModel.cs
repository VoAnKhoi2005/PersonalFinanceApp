using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;
using System.Windows;
using PersonalFinanceApp.ViewModel.LoginMenu;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceApp.Model;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class SettingDeleteAccountViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly SharedService _sharedService;
    #region Properties
    //BudgetDefault
    public string BudgetDefault {
        get => _budgetDefault;
        set {
            if (value != _budgetDefault) {
                _budgetDefault = value;
                OnPropertyChanged();
            }
        }
    }
    private string _budgetDefault;

    #endregion
    #region Command
    public ICommand CancelDeleteAccountCommand { get; set; }
    public ICommand ConfirmDeleteAccountCommand { get; set; }
    #endregion
    public SettingDeleteAccountViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _sharedService = serviceProvider.GetRequiredService<SharedService>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        CancelDeleteAccountCommand = new RelayCommand<object>(CloseModal);
        ConfirmDeleteAccountCommand = new RelayCommand<object>(DeleteUser);
    }
    public void CloseModal(object? parameter = null) {
        _modalNavigationStore.Close();
    }
    public void DeleteUser(object? w) {
        Logout();
        try {
            var item = DBManager.GetFirst<User>(u => u.UserID == int.Parse(_accountStore.UsersID));
            DBManager.Remove(item);
        }
        catch (Exception ex) {
            MessageBox.Show("Có lỗi xảy ra vui lòng thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        _modalNavigationStore.Close();
    }
    public void Logout(object? parameter = null) {

        NavigationStore navigationStore = _serviceProvider.GetRequiredService<NavigationStore>();
        navigationStore.CurrentViewModel = _serviceProvider.GetRequiredService<LoginNewAccountViewModel>();

        if (_sharedService.w != null) {
            _sharedService.m = (View.MainWindow?)Application.Current.MainWindow;
            Application.Current.MainWindow.Visibility = Visibility.Collapsed;
            Application.Current.MainWindow = _sharedService.w;
            Application.Current.MainWindow.Visibility = Visibility.Visible;
            Application.Current.MainWindow.Activate();
        }

    }
}

