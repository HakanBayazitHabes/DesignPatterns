using System.Net.Mail;
using System.Net;
using System.Net.Mime;

namespace WebApp.ChainOfResponsibility.ChangeOfResponsibility
{
    public class SendEmailProcessHandler : ProcessHandler
    {
        private readonly string _fileName;
        private readonly string _toEmail;

        public SendEmailProcessHandler(string fileName, string toEmail)
        {
            _fileName = fileName;
            _toEmail = toEmail;
        }

        public override object handle(object request)
        {
            var zipMemoryStream = request as MemoryStream;
            zipMemoryStream.Position = 0;

            var smtpClient = new SmtpClient();

            smtpClient.Host = "smtp.gmail.com";
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("hakanbayazithabes@gmail.com", "iphgphkqtpbuoftg");
            smtpClient.EnableSsl = true;

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress("hakanbayazithabes@gmail.com");
            mailMessage.To.Add("habehakan@gmail.com");

            mailMessage.Subject = "<h3>Zip Dosyası</h3>";
            mailMessage.Body = @$"
                    <h4>Zip dosyası ektedir.</h4>";

            Attachment attachment = new Attachment(zipMemoryStream, _fileName, MediaTypeNames.Application.Zip);

            mailMessage.Attachments.Add(attachment);

            mailMessage.IsBodyHtml = true;

            smtpClient.Send(mailMessage);

            return base.handle(null);
        }


    }
}
