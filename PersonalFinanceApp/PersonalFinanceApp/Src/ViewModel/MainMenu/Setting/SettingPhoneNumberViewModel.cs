using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Stores;
using System.Windows.Input;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceApp.Model;


namespace PersonalFinanceApp.ViewModel.MainMenu;

public class SettingPhoneNumberViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    #region Properties
    //PhoneNumber
    public string PhoneNumber {
        get => _phoneNumber;
        set {
            if (value != _phoneNumber) {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }
    }
    private string _phoneNumber;

    #endregion
    #region Command
    public ICommand CancelChangedPhoneNumberCommand { get; set; }
    public ICommand ConfirmChangedPhoneNumberCommand { get; set; }
    #endregion
    public SettingPhoneNumberViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        PhoneNumber = _accountStore.Users.PhoneNumber?.ToString();

        CancelChangedPhoneNumberCommand = new RelayCommand<object>(CloseModal);
        ConfirmChangedPhoneNumberCommand = new RelayCommand<object>(ChangedPhoneNumber);
    }
    public void CloseModal(object? parameter = null) {
        _modalNavigationStore.Close();
    }
    public void ChangedPhoneNumber(object? parameter = null) {
        try {
            var user = DBManager.GetFirst<User>(u => u.UserID == _accountStore.Users.UserID);
            if (user != null) { 
                user.PhoneNumber = PhoneNumber;
            }
            bool check = DBManager.Update(user);
            if (!check) throw new Exception("Update Phone Numbers failed");
        }
        catch (Exception ex) {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        _modalNavigationStore.Close();
    }
}
