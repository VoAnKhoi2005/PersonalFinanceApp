using Microsoft.Extensions.DependencyInjection;
using OxyPlot;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class SettingChangedEmailViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    #region Properties
    //email current
    public string EmailCurrent {
        get => _emailCurrent;
        set {
            if (value != _emailCurrent) {
                _emailCurrent = value;
                OnPropertyChanged();
            }
        }
    }
    private string _emailCurrent;
    //email new
    public string EmailChanged {
        get => _emailChanged;
        set {
            if (value != _emailChanged) {
                _emailChanged = value;
                OnPropertyChanged();
            }
        }
    }
    private string _emailChanged;
    //invalid email new
    public bool CorrectEmailNew {
        get => _currentEmailNew;
        set {
            if (_currentEmailNew != value) {
                _currentEmailNew = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _currentEmailNew;
    //invalid email current
    public bool CorrectEmailCurrent {
        get => _correctEmailCurrent;
        set {
            if (_correctEmailCurrent != value) {
                _correctEmailCurrent = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _correctEmailCurrent;
    #endregion
    #region Command
    public ICommand ChangedEmailCommand { get; set; }
    public ICommand CancelChangedEmailCommand { get; set; }
    #endregion
    public SettingChangedEmailViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;

        _accountStore = serviceProvider.GetRequiredService<AccountStore>();
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();

        ChangedEmailCommand = new RelayCommand<object>(ChangedEmail);
        CancelChangedEmailCommand = new RelayCommand<object>(CloseModal);
        CorrectEmailNew = false;
        CorrectEmailCurrent = false;
    }
    public void CloseModal(object? parameter = null) {
        _modalNavigationStore.Close();
    }
    public void ChangedEmail(object? parameter = null) {
        try {
            CorrectEmailNew = false;
            CorrectEmailCurrent = false;
            var item = DBManager.GetFirst<User>(u => u.UserID == int.Parse(_accountStore.UsersID));
            if (item.Email.CompareTo(EmailCurrent) != 0) {
                CorrectEmailCurrent = true;
                throw new Exception("Gmail không tồn tại!");
            }
            string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            if (!Regex.IsMatch(EmailChanged, pattern)) {
                CorrectEmailCurrent = true;
                throw new Exception("Ôi Gmail bạn nhập tệ quá!");
            }
            item.Email = EmailChanged;

            bool check = DBManager.Update(item);

            if (!check) {
                throw new ArgumentException();
            }
        }
        catch (Exception ex) {
            MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        _modalNavigationStore.Close();
    }
}
