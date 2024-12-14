using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PersonalFinanceApp.ViewModel.MainMenu;

public class SettingChangedPasswordViewModel : BaseViewModel {
    private readonly ModalNavigationStore _modalNavigationStore;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccountStore _accountStore;
    #region Properties
    //password current
    public string PasswordCurrent {
        get => _passwordCurrent;
        set {
            if (value != _passwordCurrent) {
                _passwordCurrent = value;
                OnPropertyChanged();
            }
        }
    }
    private string _passwordCurrent;
    //password new
    public string PasswordNew {
        get => _passwordNew;
        set {
            if (value != _passwordNew) {
                _passwordNew = value;
                OnPropertyChanged();
            }
        }
    }
    private string _passwordNew;
    //confirm password
    public string ConfirmPassword {
        get => _confirmPassword;
        set {
            if (value != _confirmPassword) {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }
    }
    private string _confirmPassword;
    //check password current
    public bool CurrentPwr {
        get => _currentPwr;
        set {
            if (value != _currentPwr) {
                _currentPwr = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _currentPwr;
    //check password new
    public bool NewPwr {
        get => _newPwr;
        set {
            if (value != _newPwr) {
                _newPwr = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _newPwr;
    //check password confirm
    public bool ConfirmPwr {
        get => _confirmPwr;
        set {
            if (value != _confirmPwr) {
                _confirmPwr = value;
                OnPropertyChanged();
            }
        }
    }
    private bool _confirmPwr;
    #endregion
    #region Command
    public ICommand ChangedPasswordCommand { get; set; }
    public ICommand CancelChangedPasswordCommand { get; set; }
    public ICommand ConfirmPasswordCommand {  get; set; }
    public ICommand PasswordNewCommand { get;set; }
    #endregion
    public SettingChangedPasswordViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _accountStore = serviceProvider.GetRequiredService<AccountStore>();

        ChangedPasswordCommand = new RelayCommand<object>(ChangedPassword);
        CancelChangedPasswordCommand = new RelayCommand<object>(CloseModal);

        PasswordNewCommand = new RelayCommand<PasswordBox>(p => PasswordNew = p.Password);
        ConfirmPasswordCommand = new RelayCommand<PasswordBox>(p => ConfirmPassword = p.Password);

        CurrentPwr = false;
        NewPwr = false;
        ConfirmPwr = false;
    }
    public void CloseModal(object? parameter = null) {
        _modalNavigationStore.Close();
    }
    public void ChangedPassword(object? parameter = null) {
        CurrentPwr = false;
        NewPwr = false;
        ConfirmPwr = false;
        if (VerifyPassword(PasswordCurrent)) {
            try {
                string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
                if(!Regex.IsMatch(PasswordNew, pattern)) {
                    NewPwr = true;
                    throw new Exception("Password bạn nhập tệ quá đi mất thôi");
                }
                if (PasswordNew.CompareTo(ConfirmPassword) != 0) {
                    ConfirmPwr = true;
                    throw new Exception("Password không khớp");
                }

                var item = DBManager.GetFirst<User>(u => u.UserID == int.Parse(_accountStore.UsersID));

                bool changed = item.ChangePassword(PasswordNew);

                if (!changed) { throw new ArgumentException(); }
            }
            catch (Exception ex) {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }
        else {
            CurrentPwr = true;
            MessageBox.Show("Nhập sai password cũ rồi nhe", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        _modalNavigationStore.Close();
    }
    public bool VerifyPassword(string passwrd) {
        User usr = DBManager.GetFirst<User>(u => u.UserID == int.Parse(_accountStore.UsersID));
        if (usr == null) {
            return false;
        }
        return usr.VerifyPassword(passwrd);
    }
}
