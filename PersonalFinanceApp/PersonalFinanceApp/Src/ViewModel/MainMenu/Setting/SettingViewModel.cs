using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.LoginMenu;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows;
using System.Windows.Input;
using Application = System.Windows.Application;
using PersonalFinanceApp.ViewModel.Command;
using Windows.ApplicationModel.VoiceCommands;
using Microsoft.Extensions.Logging.Abstractions;
using PersonalFinanceApp.Src.ViewModel;

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
    public ICommand ChangedBudgetDefaultCommand { get; set; }
    public ICommand ChangedThemeLightCommand {  get; set; }
    public ICommand ChangedThemeDarkCommand {  get; set; }
    #endregion
    public SettingViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _sharedService = serviceProvider.GetRequiredService<SharedService>();

        ChangedYourEmailCommand = new NavigateModalCommand<SettingChangedEmailViewModel>(serviceProvider);
        ChangedYourPasswordCommand = new NavigateModalCommand<SettingChangedPasswordViewModel>(serviceProvider);
        ExportFileCommand = new NavigateModalCommand<SettingExportToExcelViewModel>(serviceProvider);
        ChangedBudgetDefaultCommand = new NavigateModalCommand<SettingBudgetDefaultViewModel>(serviceProvider);
        DeleteYourAccountCommand = new NavigateModalCommand<SettingDeleteAccountViewModel>(serviceProvider);
        LogOutCommand = new NavigateModalCommand<SettingLogoutViewModel>(serviceProvider);
        ChangedThemeLightCommand = new RelayCommand<object>(ChangedThemeLight);
        ChangedThemeDarkCommand = new RelayCommand<object>(ChangedThemeDark);
    }
    public void ChangedThemeLight(object? parameter = null) {
        Apptheme.ChangeTheme(new Uri("Resources/Light.xaml", UriKind.Relative));
    }
    public void ChangedThemeDark(object? parameter = null) {
        Apptheme.ChangeTheme(new Uri("Resources/Dark.xaml", UriKind.Relative));
    }
}
