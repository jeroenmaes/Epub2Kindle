using Epub2Kindle.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Epub2Kindle.Test
{
    [TestClass]
    public class MailerTest
    {
        [TestMethod]
        public void SendMail()
        {
            var mailer = new Mailer();
            var email = new Email();

            email.Address = "";
            email.DisplayName = "";
            email.Password = "";

            email.Host = "";
            email.Port = 587;
            email.To = "";
            email.Subject = "";
            email.AttachmentPaths = new List<string>();
            email.AttachmentPaths.Add(@"book.mobi");

            mailer.Send(email);

            
        }
    }
}
