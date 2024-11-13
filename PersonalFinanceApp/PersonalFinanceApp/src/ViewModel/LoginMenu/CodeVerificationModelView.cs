using System.Windows.Input;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class CodeVerificationModelView : BaseViewModel
{
    #region Properties
    public bool IncorrectVerify { get; set; } = false;
    private string _number1;
    public string Number1 {
        get => _number1;
        set {
            _number1 = value;
            OnPropertyChanged();
        }
    }
    private string _number2;
    public string Number2 {
        get => _number2;
        set {
            _number2 = value;
            OnPropertyChanged();
        }
    }
    private string _number3;
    public string Number3 {
        get => _number3;
        set {
            _number3 = value;
            OnPropertyChanged();
        }
    }
    private string _number4;
    public string Number4 {
        get => _number4;
        set {
            _number4 = value;
            OnPropertyChanged();
        }
    }
    private string _number5;
    public string Number5 {
        get => _number5;
        set {
            _number5 = value;
            OnPropertyChanged();
        }
    }
    private string _number6;
    public string Number6 {
        get => _number6;
        set {
            _number6 = value;
            OnPropertyChanged();
        }
    }
    #endregion

    public ICommand NavigationConfirmCodeCommand { get; set; }

    public CodeVerificationModelView(LoginNavigationStore navigationStore)
    {
        NavigationConfirmCodeCommand = new ConfirmCodeCommand(navigationStore, this);
    }

    public bool VerifyCode()
    {
        return true;
    }
}