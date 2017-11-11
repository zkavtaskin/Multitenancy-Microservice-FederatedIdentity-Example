using Server.Core.Domain;

namespace Server.Domain.Tenants
{
    public class TenantCreated : IDomainEvent
    {
        public Tenant Tenant { get; private set; } 
        public string InitUserEmail { get; private set; }

        public TenantCreated(Tenant tenant, string initUserEmail)
        {
            this.Tenant = tenant;
            this.InitUserEmail = initUserEmail;
        }
    }
}
