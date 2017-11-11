using Server.Core.ConfigManager;
using Server.Core.Domain;
using Server.Core.Message.Email;
using Server.Core.Repository;
using Server.Core.Time;
using Server.Domain.Tenants;
using Server.Service;

namespace Server.Domain.Users
{
    public class UserInvitedHandler : Handles<UserInvited>
    {
        IEmailDispatcher emailDispatcher;
        IConfigurationProvider configProvider;
        MessageGenerator<EmailTemplate, EmailMessage> emailMsgGenerator;
        IRepository<User> userRepository;
        IRepository<Tenant> tenantRepository;
        TenantContext tenantContext;

        public UserInvitedHandler(MessageGenerator<EmailTemplate, EmailMessage> emailMsgGenerator, 
            IEmailDispatcher emailDispatcher, 
            IConfigurationProvider configurationProvider,
            IRepository<User> userRepository,
            IRepository<Tenant> tenantRepository,
            TenantContext tenantContext)
        {
            this.emailMsgGenerator = emailMsgGenerator;
            this.emailDispatcher = emailDispatcher;
            this.configProvider = configurationProvider;
            this.userRepository = userRepository;
            this.tenantRepository = tenantRepository;
            this.tenantContext = tenantContext;
        }

        public void Handle(UserInvited args)
        {
            Tenant tenant = this.tenantRepository.FindById(this.tenantContext.ID);
            User userInvitedBy = this.userRepository.FindById(args.Invite.InvitedByUserId);

            EmailMessage msg =
                this.emailMsgGenerator.Generate(Message.Email.EmailTemplate.ResourceManager, 
                Server.Domain.Message.Email.Template.UserInvite.ToString(), new 
                {
                    username = args.Invite.Email,
                    org = tenant.Name.ToUpper(),
                    org_friendly = tenant.NameFriendly,
                    inviter =  userInvitedBy.FullName,
                    url = this.configProvider.GetSetting(ConfigKeys.Url),
                    key = args.Invite.InviteKey,
                    year = TimeProvider.Current.UtcNow.Year
                });

            this.emailDispatcher.Dispatch(new Email(this.configProvider.GetSetting(ConfigKeys.EmailFrom), args.Invite.Email, msg));
        }
    }
}
