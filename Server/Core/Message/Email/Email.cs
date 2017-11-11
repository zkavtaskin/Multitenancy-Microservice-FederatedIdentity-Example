using System.Collections.Generic;

namespace Server.Core.Message.Email
{
    public class Email
    {
        public string From { get; private set; }
        public List<string> To { get; private set; }
        public List<string> CC { get; private set; }
        public EmailMessage Message { get; private set; }

        public Email()
        {

        }

        public Email(string from, List<string> to, EmailMessage msg)
            : this(from, to, new List<string>(), msg)
        {
        }

        public Email(string from, string to, EmailMessage msg)
            : this(from, new List<string>() { to }, msg)
        {

        }

        public Email(string from, List<string> to, List<string> cc, EmailMessage msg)
        {
            this.From = from;
            this.To = to;
            this.CC = cc;
            this.Message = msg;
        }
    }
}
