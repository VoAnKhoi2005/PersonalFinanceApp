using System.Data.Common;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Behaviors;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using XAct;
using static MaterialDesignThemes.Wpf.Theme;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class CodeVerificationViewModel : BaseViewModel {
    #region Properties
    public bool flag = false;
    public bool IncorrectVerify { get; set; } = false;
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
        NavigationConfirmCodeCommand = new NavigateCommand<CreateNewPasswordViewModel>(serviceProvider, null, VerifyCode);
     
        FocusNextCommand = new RelayCommand<System.Windows.Controls.TextBox>( p => FocusNext(p));
        FocusPreviousCommand = new RelayCommand<System.Windows.Controls.TextBox>(p => FocusPrevious(p));
        LoadedCommand = new RelayCommand<System.Windows.Controls.TextBox>(p => FocusFirst(p));
    }

    public bool VerifyCode()
    {

        return true;
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

