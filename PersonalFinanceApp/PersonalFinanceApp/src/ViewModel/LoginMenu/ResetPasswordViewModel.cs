using System.Net;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PersonalFinanceApp.Database;
using PersonalFinanceApp.Model;
using PersonalFinanceApp.ViewModel.Command;
using PersonalFinanceApp.ViewModel.Stores;

namespace PersonalFinanceApp.ViewModel.LoginMenu;

public class ResetPasswordViewModel : BaseViewModel
{
    private readonly IServiceProvider _serviceProvider;
    private readonly SharedDataService _sharedDataService;

    #region Properties
    public static string _to;
    public string _randomcode;

    private bool _incorrectUserGmail = false;
    public bool IncorrectUserGmail {
        get => _incorrectUserGmail;
        set {
            _incorrectUserGmail = value;
            OnPropertyChanged();
        }
    }
    private string _userNameReset;
    public string UserNameReset {
        get => _userNameReset;
        set {
            _userNameReset = value;
            OnPropertyChanged();
        }
    }
    private string _gmailReset;
    public string GmailReset {
        get => _gmailReset;
        set {
            _gmailReset = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region Command
    public ICommand NavigateConfirmEmailCommand { get; set; }
    public ICommand CancelEmailCommand { get; set; }

    #endregion

    public ResetPasswordViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _sharedDataService = serviceProvider.GetRequiredService<SharedDataService>();
        NavigateConfirmEmailCommand = new NavigateCommand<CodeVerificationViewModel>(serviceProvider, VerifyEmail);
        CancelEmailCommand = new NavigateCommand<LoginNewAccountViewModel>(serviceProvider);
    }

    public bool VerifyEmail()
    {
        var usr = DBManager.GetFirst<User>(u => u.Username == UserNameReset && u.Email == GmailReset);
        if (usr != null) {
            _sharedDataService.UserName = (UserNameReset.ToString());
            RandomCode();
            IncorrectUserGmail = false;
            _sharedDataService.RandomCode = (_randomcode.ToString());
            return true;
        }
        IncorrectUserGmail = true;
        return false;
    }

    private void RandomCode() {
        string from, pass, messagebody;
        Random rd = new Random();
        _randomcode = rd.Next(100000, 1000000).ToString();
        MailMessage message = new MailMessage();
        _to = GmailReset;
        from = "personalfinanceapplicationuit@gmail.com";
        pass = "knfm aeqy bjhs xhpx";
        messagebody = $"Your reset Code is {_randomcode}";
        message.To.Add(_to);
        message.From = new MailAddress(from);
        message.Body = messagebody;
        message.Subject = "Password Reset Code";
        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
        smtp.EnableSsl = true;
        smtp.Port = 587;
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.Credentials = new NetworkCredential(from, pass);
        try {
            smtp.Send(message);
        }catch(Exception ex) {
            MessageBox.Show(ex.Message);
        }
    }
}