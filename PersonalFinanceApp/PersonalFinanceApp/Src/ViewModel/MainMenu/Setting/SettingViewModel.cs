﻿using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;

namespace PersonalFinanceApp.Src.ViewModel.MainMenu.Setting;
public class SettingViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
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
        
    }
}
