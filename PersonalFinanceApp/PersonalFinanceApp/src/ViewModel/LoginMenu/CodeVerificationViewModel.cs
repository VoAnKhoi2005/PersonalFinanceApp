using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class CodeVerificationViewModel : BaseViewModel {
    private readonly IServiceProvider _serviceProvider;
    private readonly SharedDataService _sharedDataService;
    #region Properties
    public string code;
    public bool flag = false;
    private bool _incorrectVerify = false;
    public bool IncorrectVerify {
        get => _incorrectVerify;
        set {
            _incorrectVerify = value;
            OnPropertyChanged();
        }
    }

    private string _verify1 ;
    public string Verify1 {
        get => _verify1;
        set {
            _verify1 = value;
            OnPropertyChanged();
        }
    }
    private string _verify2;
    public string Verify2 {
        get => _verify2;
        set {
            _verify2 = value;
            OnPropertyChanged();
        }
    }
    private string _verify3;
    public string Verify3 {
        get => _verify3;
        set {
            _verify3 = value;
            OnPropertyChanged();
        }
    }
    private string _verify4;
    public string Verify4 {
        get => _verify4;
        set {
            _verify4 = value;
            OnPropertyChanged();
        }
    }
    private string _verify5;
    public string Verify5 {
        get => _verify5;
        set {
            _verify5 = value;
            OnPropertyChanged();
        }
    }
    private string _verify6;
    public string Verify6 {
        get => _verify6;
        set {
            _verify6 = value;
            OnPropertyChanged();
        }
    }
    #endregion
      
    #region Command
    public ICommand NavigationConfirmCodeCommand { get; set; }
    public ICommand FocusNextCommand { get; set; }
    public ICommand FocusPreviousCommand {  get; set; }
    public ICommand LoadedCommand { get; set; }
    #endregion
    public CodeVerificationViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _sharedDataService = serviceProvider.GetRequiredService<SharedDataService>();
        NavigationConfirmCodeCommand = new NavigateCommand<CreateNewPasswordViewModel>(serviceProvider, VerifyCode);
        //Focus
        FocusNextCommand = new RelayCommand<System.Windows.Controls.TextBox>( p => FocusNext(p));
        FocusPreviousCommand = new RelayCommand<System.Windows.Controls.TextBox>(p => FocusPrevious(p));
        LoadedCommand = new RelayCommand<System.Windows.Controls.TextBox>(p => FocusFirst(p));
    }

    public bool VerifyCode()
    {
        code = _sharedDataService.RandomCode;
        if (Verify1.Length < 1 || Verify2.Length < 1 || Verify3.Length < 1 || Verify4.Length < 1 || Verify5.Length < 1 || Verify6.Length < 1) {
            IncorrectVerify = true;
            return false;
        }
        else {
            string verifycode = Verify1 + Verify2 + Verify3 + Verify4 + Verify5 + Verify6;
            if (verifycode.CompareTo(code) == 0) {
                IncorrectVerify = false;
                return true;
            }
            IncorrectVerify = true;
            return false;
        }
    }
    private void FocusNext(object parameter) {
        System.Windows.Controls.TextBox tb = parameter as System.Windows.Controls.TextBox;
        if (tb != null) {
            if (!flag) tb?.Focus();
        }
    }
    private void FocusPrevious(object parameter) {
        System.Windows.Controls.TextBox tb = parameter as System.Windows.Controls.TextBox;
        if (tb != null) {
            if (tb.Name.CompareTo("verify5") == 0) Verify6 = string.Empty;
            if (tb.Name.CompareTo("verify4") == 0) Verify5 = string.Empty;
            if (tb.Name.CompareTo("verify3") == 0) Verify4 = string.Empty;
            if (tb.Name.CompareTo("verify2") == 0) Verify3 = string.Empty;
            if (tb.Name.CompareTo("verify1") == 0) Verify2 = string.Empty;
            if (tb.Name.CompareTo("temporary") == 0) { flag = true; Verify1 = string.Empty; }
            tb?.Focus();
            flag = false;
        }
    }
    private void FocusFirst(object parameter) {
        System.Windows.Controls.TextBox tb = parameter as System.Windows.Controls.TextBox;
        if (tb != null) { tb.Focus(); }
    }
}

