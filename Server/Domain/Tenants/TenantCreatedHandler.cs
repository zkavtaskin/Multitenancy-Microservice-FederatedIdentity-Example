using Server.Core.ConfigManager;
using Server.Core.Domain;
using Server.Core.Message.Email;
using Server.Core.Time;
using Server.Domain.Tenants;

namespace Server.Domain.Stops
{
    public class TenantCreatedHandler : Handles<TenantCreated>
    {
        MessageGenerator<EmailTemplate, EmailMessage> emailMsgGenerator;
        IEmailDispatcher emailDispatcher;
        IConfigurationProvider configProvider;

        public TenantCreatedHandler(MessageGenerator<EmailTemplate, EmailMessage> emailMsgGenerator,
            IEmailDispatcher emailDispatcher, 
            IConfigurationProvider configProvider)
        {
            this.emailMsgGenerator = emailMsgGenerator;
            this.emailDispatcher = emailDispatcher;
            this.configProvider = configProvider;
        }

        public void Handle(TenantCreated args)
        {
            EmailMessage msg =
            this.emailMsgGenerator.Generate(Message.Email.EmailTemplate.ResourceManager,
                Server.Domain.Message.Email.Template.TenantCreated.ToString(), new
                {
                    email = args.InitUserEmail,
                    key = args.Tenant.Id,
                    url = this.configProvider.GetSetting(ConfigKeys.Url),
                    org =  args.Tenant.Name.ToUpper(),
                    org_friendly = args.Tenant.NameFriendly,
                    year = TimeProvider.Current.UtcNow.Year
                });

            this.emailDispatcher.Dispatch(new Email(this.configProvider.GetSetting(ConfigKeys.EmailFrom), args.InitUserEmail, msg));
        }
    }
}
