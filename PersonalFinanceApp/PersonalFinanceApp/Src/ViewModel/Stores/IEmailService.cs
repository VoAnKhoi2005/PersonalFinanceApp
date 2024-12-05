using System.Net.Mail;
using System.Net;
using System.Windows;

namespace PersonalFinanceApp.Src.ViewModel.Stores;
public interface IEmailService {
}

public class EmailService : IEmailService {
    public void SendResetCodeAsync(string to, string code) {
        string from = "personalfinanceapplicationuit@gmail.com";
        string pass = "knfm aeqy bjhs xhpx"; 
        string messageBody = $"Your reset code is {code}";

        MailMessage message = new MailMessage();
        message.To.Add(to);
        message.From = new MailAddress(from);
        message.Body = messageBody;
        message.Subject = "Password Reset Code";

        SmtpClient smtp = new SmtpClient("smtp.gmail.com") {
            EnableSsl = true,
            Port = 587,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential(from, pass)
        };
        try {
             smtp.SendMailAsync(message); 
        }
        catch (Exception ex) {
            MessageBox.Show($"Error sending email: {ex.Message}");
        }
    }
}

