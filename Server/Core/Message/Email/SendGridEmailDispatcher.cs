using System.Net;
using System.Net.Mail;
using SendGrid;
using Server.Core.ConfigManager;
using Server.Domain;

namespace Server.Core.Message.Email
{
    public class SendGridEmailDispatcher : IEmailDispatcher
    {
        IConfigurationProvider configurationProvider;

        public SendGridEmailDispatcher(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public void Dispatch(Email message)
        {
            SendGridMessage sndGrdMsg = new SendGridMessage();
            sndGrdMsg.From = new MailAddress(message.From);
            sndGrdMsg.AddTo(message.To);

            sndGrdMsg.Subject = message.Message.Subject;
            sndGrdMsg.Html = message.Message.Body;
            sndGrdMsg.Text = message.Message.BodyPlainText;

            NetworkCredential credentials = 
                new NetworkCredential(configurationProvider.GetSetting(ConfigKeys.EmailServerUsername), 
                    configurationProvider.GetSetting(ConfigKeys.EmailServerPassword));

            SendGrid.Web transportWeb = new SendGrid.Web(credentials);
            transportWeb.Deliver(sndGrdMsg);
        }
    }
}
