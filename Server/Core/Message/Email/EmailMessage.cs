

namespace Server.Core.Message.Email
{
    public class EmailMessage : Message
    {
        public string Subject { get; set; }
        public string BodyPlainText { get; set; }
    }
}
