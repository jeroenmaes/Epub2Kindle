using Epub2Kindle.Models;
using System.Net;
using System.Net.Mail;

namespace Epub2Kindle
{
    public class Mailer
    {
        public void Send(Email email)
        {
            MailMessage mail = CreateMailMessage(email);
            SmtpClient client = CreateSmptClient(email);

            client.Send(mail);
        }

        private static SmtpClient CreateSmptClient(Email email)
        {
            return new SmtpClient()
            {
                Port = email.Port,
                Host = email.Host,
                //EnableSsl = true,
                //DeliveryMethod = SmtpDeliveryMethod.Network,
                //UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email.Address, email.Password)
            };
        }

        private static MailMessage CreateMailMessage(Email email)
        {
            var mail = new MailMessage()
            {
                From = new MailAddress(email.Address, email.DisplayName),
                Subject = email.Subject
            };
            mail.To.Add(new MailAddress(email.To));
            foreach (var attachmentPath in email.AttachmentPaths)
            {
                mail.Attachments.Add(new Attachment(attachmentPath));
            }

            return mail;
        }

    }
}
