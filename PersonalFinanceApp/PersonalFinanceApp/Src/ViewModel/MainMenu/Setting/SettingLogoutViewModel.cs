using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using PersonalFinanceApp.ViewModel.LoginMenu;
using CommunityToolkit.Mvvm.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class SettingLogoutViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly SharedService _sharedService;
    #region Command
    public ICommand CancelLogoutCommand { get; set; }
    public ICommand ConfirmLogoutCommand { get; set; }
    #endregion
    public SettingLogoutViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _sharedService = serviceProvider.GetRequiredService<SharedService>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        CancelLogoutCommand = new RelayCommand<object>(CloseModal);
        ConfirmLogoutCommand = new RelayCommand<object>(Logout);
    }
    public void CloseModal(object? parameter = null) {
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
        _modalNavigationStore.Close();

    }
}
