using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Excel;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.LoginMenu;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Windows.UI.Notifications;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;
using Window = System.Windows.Window;
using Application = System.Windows.Application;
using PersonalFinanceApp.ViewModel.Command;

namespace PersonalFinanceApp.ViewModel.MainMenu;
public class SettingViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    private readonly SharedService _sharedService;
    #region Properties

    #endregion
    #region Command
    public ICommand ChangedYourEmailCommand {  get; set; }
    public ICommand ChangedYourPasswordCommand {  get; set; }
    public ICommand DeleteYourAccountCommand { get; set; }
    public ICommand LogOutCommand {  get; set; }
    public ICommand ExportFileCommand { get; set; }
    #endregion
    public SettingViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _sharedService = serviceProvider.GetRequiredService<SharedService>();

        ChangedYourEmailCommand = new NavigateModalCommand<SettingChangedEmailViewModel>(serviceProvider);
        ChangedYourPasswordCommand = new NavigateModalCommand<SettingChangedPasswordViewModel>(serviceProvider);
        ExportFileCommand = new NavigateModalCommand<SettingExportToExcelViewModel>(serviceProvider);

        LogOutCommand = new RelayCommand<object>(Logout);

        DeleteYourAccountCommand = new RelayCommand<object>(DeleteUser);
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
