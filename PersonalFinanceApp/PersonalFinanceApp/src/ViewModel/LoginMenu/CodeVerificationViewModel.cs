using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;
using static MaterialDesignThemes.Wpf.Theme;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class CodeVerificationViewModel : BaseViewModel
{
    #region Properties
    public bool IncorrectVerify { get; set; } = false;
    private string _verify1;
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
    public ICommand NavigationConfirmCodeCommand { get; set; }
    public ICommand FocusNextCommand { get; set; }
    public ICommand FocusPreviousCommand { get; set; }

    public CodeVerificationViewModel(IServiceProvider serviceProvider) {
        NavigationConfirmCodeCommand = new NavigateCommand<CreateNewPasswordViewModel>(
            serviceProvider.GetRequiredService<NavigationStore>(),
            () => serviceProvider.GetRequiredService<CreateNewPasswordViewModel>(),
            VerifyCode
            );

        FocusNextCommand = new RelayCommand<System.Windows.Controls.TextBox>(p => { return true; }, p => { FocusTextBox(p); });
    }

    public bool VerifyCode()
    {
        return true;
    }
    private void FocusTextBox(object parameter) {
        System.Windows.Controls.TextBox tb = parameter as System.Windows.Controls.TextBox;
        if (tb != null) {
            tb?.Focus();
        }
    }

}