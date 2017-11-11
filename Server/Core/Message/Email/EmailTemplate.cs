
namespace Server.Core.Message.Email
{
    public class EmailTemplate : MessageTemplate
    {
        public string Subject { get; set; }
        public string BodyHtml { get; set; }
        public string BodyText { get; set; }
    }
}
