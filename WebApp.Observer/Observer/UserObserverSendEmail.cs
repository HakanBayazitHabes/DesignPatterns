using BaseProject.Models;
using System.Net;
using System.Net.Mail;

namespace WebApp.Observer.Observer
{
    public class UserObserverSendEmail : IUserObserver
    {
        private readonly IServiceProvider _serviceProvider;

        public UserObserverSendEmail(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void UserCreatedAsync(AppUser appUser)
        {
            var logger = _serviceProvider.GetRequiredService<ILogger<UserObserverSendEmail>>();

            var smtpClient = new SmtpClient();

            smtpClient.Host = "smtp.gmail.com";
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("<email>", "<password>");
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress("<email>");
            mailMessage.To.Add("<email>");

            mailMessage.Subject = "<h3>Hoşgeldiniz</h3>";
            mailMessage.Body = @$"
                    <h4>Şifrenizi yenilemek için aşağıdaki linke tıklayınız.</h4>";
            mailMessage.IsBodyHtml = true;

            smtpClient.SendMailAsync(mailMessage);

            logger.LogInformation($"Email sent to {appUser.Email}");
        }

        
    }
}
