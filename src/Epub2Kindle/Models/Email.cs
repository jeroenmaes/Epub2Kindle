using System;
using System.Collections.Generic;
using System.Text;

namespace Epub2Kindle.Models
{
    public class Email
    {
        public string To { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Subject { get; set; }
        public List<string> AttachmentPaths { get; set; }
        public string DisplayName { get; set; }
    }
}
