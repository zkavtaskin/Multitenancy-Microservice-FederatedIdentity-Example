using System.Collections.Generic;

namespace Server.Core.Message.Email
{
    public class EmailMessageFormater : MessageFormater<EmailTemplate, EmailMessage>
    {
        public override EmailMessage Format(EmailTemplate template, List<KeyValuePair<string, string>> tokensWithValues)
        {
            EmailMessage msg = new EmailMessage()
            {
                Body = template.BodyHtml,
                BodyPlainText = template.BodyText,
                Subject = template.Subject
            };

            foreach (KeyValuePair<string, string> token in tokensWithValues)
            {
                msg.Subject = msg.Subject.Replace(token.Key, token.Value);
                msg.Body = msg.Body.Replace(token.Key, token.Value);
                msg.BodyPlainText = msg.BodyPlainText.Replace(token.Key, token.Value);
            }
            return msg;
        }
    }
}
