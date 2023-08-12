using MediatR;
using System.Net.Mail;
using System.Net;
using WebApp.Observer.Events;
using WebApp.Observer.Observer;

namespace WebApp.Observer.EventHandlers
{
    public class SendEmailEventHandler : INotificationHandler<UserCreatedEvent>
    {
        private readonly ILogger<SendEmailEventHandler> _logger;

        public SendEmailEventHandler(ILogger<SendEmailEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            var smtpClient = new SmtpClient();

            smtpClient.Host = "smtp.gmail.com";
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("<email>", "<password>");
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(notification.AppUser.Email);
            mailMessage.To.Add(new MailAddress(notification.AppUser.Email));

            mailMessage.Subject = "<h3>Hoşgeldiniz</h3>";
            mailMessage.Body = @$"
                    <h4>Şifrenizi yenilemek için aşağıdaki linke tıklayınız.</h4>";
            mailMessage.IsBodyHtml = true;

            smtpClient.SendMailAsync(mailMessage);

            _logger.LogInformation($"Email sent to {notification.AppUser.Email}");

            return Task.CompletedTask;
        }
    }
}
