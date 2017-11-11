using Server.Core.ConfigManager;
using Server.Core.Domain;
using Server.Core.Message.Email;
using Server.Core.Repository;
using Server.Core.Time;
using Server.Domain.Groups;
using Server.Domain.Tenants;
using Server.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using Server.Service;

namespace Server.Domain.Stops
{
    public class StoppedHandler : Handles<Stopped>
    {
        MessageGenerator<EmailTemplate, EmailMessage> emailMsgGenerator;
        IEmailDispatcher emailDispatcher;
        IConfigurationProvider configProvider;
        IRepository<User> userRepository;
        IRepository<Group> groupRepository;
        IRepository<Tenant> tenantRepository;
        TenantContext tenantContext;

        public StoppedHandler(MessageGenerator<EmailTemplate, EmailMessage> emailMsgGenerator,
            IEmailDispatcher emailDispatcher, 
            IConfigurationProvider configProvider, 
            IRepository<User> userRepository, 
            IRepository<Group> groupRepository,
            IRepository<Tenant> tenantRepository,
            TenantContext tenantContext)
        {
            this.emailMsgGenerator = emailMsgGenerator;
            this.emailDispatcher = emailDispatcher;
            this.configProvider = configProvider;
            this.userRepository = userRepository;
            this.groupRepository = groupRepository;
            this.tenantRepository = tenantRepository;
            this.tenantContext = tenantContext;
        }

        public void Handle(Stopped args)
        {
            Tenant tenant = this.tenantRepository.FindById(this.tenantContext.ID);

            Group group = this.groupRepository.FindById(args.Stop.GroupId);

            User userStopper = this.userRepository.FindById(args.Stop.ById);

            IEnumerable<User> userReceivers = group.Users.Where(user => user.Id != userStopper.Id);

            DateTime stopDate = TimeZoneInfo.ConvertTimeFromUtc(args.Stop.Date,
                        TimeZoneInfo.FindSystemTimeZoneById(tenant.TimeZoneId));

            EmailMessage msg =
            this.emailMsgGenerator.Generate(Message.Email.EmailTemplate.ResourceManager,
                Server.Domain.Message.Email.Template.Stop.ToString(), new
                {
                    line = args.Stop.GroupName,
                    stopper = userStopper.FullName,
                    datetime = stopDate.ToString("HH:mm dddd MMMM d"),
                    problem = args.Stop.Problem,
                    stopper_number = userStopper.Contacts.First().Value,
                    url = this.configProvider.GetSetting(ConfigKeys.Url),
                    org_friendly =  tenant.NameFriendly,
                    year = TimeProvider.Current.UtcNow.Year
                });
    
            this.emailDispatcher.Dispatch(new Email(
                    this.configProvider.GetSetting(ConfigKeys.EmailFrom), 
                    userReceivers.Select(x => x.Email).ToList(), 
                    msg)
                );
        }
    }
}
