using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.Src.ViewModel.Stores;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class ResetPasswordViewModel : BaseViewModel
{
    private readonly IServiceProvider _serviceProvider;
    private readonly SharedDataService _sharedDataService;
    private readonly EmailService _emailService;
    private readonly ModalNavigationStore _modalNavigationStore;

    #region Properties
    public static string _to;
    public string _randomcode;

    public bool IncorrectUserGmail {
        get => _incorrectUserGmail;
        set {
            _incorrectUserGmail = value;
            OnPropertyChanged();
        }
    }
    private bool _incorrectUserGmail = false;
    public string UserNameReset {
        get => _userNameReset;
        set {
            _userNameReset = value;
            OnPropertyChanged();
        }
    }
    private string _userNameReset;
    public string GmailReset {
        get => _gmailReset;
        set {
            _gmailReset = value;
            OnPropertyChanged();
        }
    }
    private string _gmailReset;
    #endregion

    #region Command
    public ICommand NavigateConfirmEmailCommand { get; set; }
    public ICommand CancelEmailCommand { get; set; }

    #endregion

    public ResetPasswordViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _modalNavigationStore = serviceProvider.GetRequiredService<ModalNavigationStore>();
        _sharedDataService = serviceProvider.GetRequiredService<SharedDataService>();
        _emailService = serviceProvider.GetRequiredService<EmailService>();

        NavigateConfirmEmailCommand = new NavigateCommand<CodeVerificationViewModel>(serviceProvider, VerifyEmailAsync);

        CancelEmailCommand = new NavigateCommand<LoginNewAccountViewModel>(serviceProvider);
    }
    public bool VerifyEmailAsync()
    {
        var usr = DBManager.GetFirst<User>(u => u.Username == UserNameReset && u.Email == GmailReset);
        if (usr != null) {
            _sharedDataService.UserName = (UserNameReset.ToString());
            _randomcode = GenerateRandomCode();
            IncorrectUserGmail = false;
            _sharedDataService.RandomCode = (_randomcode.ToString());
            _emailService.SendResetCodeAsync(GmailReset, _randomcode);
            return true;
        }
        IncorrectUserGmail = true;
        return false;
    }
    public string GenerateRandomCode() {
        Random rd = new Random();
        return rd.Next(100000, 1000000).ToString();
    }
}