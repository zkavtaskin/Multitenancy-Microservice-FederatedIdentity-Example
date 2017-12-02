using Server.Service;

namespace Server.Core.Repository.Adapters.NHibernate
{
    public class NHUnitOfWorkMultitenancy : NHUnitOfWork
    {
        public NHUnitOfWorkMultitenancy(NHConfiguration nhConfiguration, TenantContext tenantContext) 
            : base(nhConfiguration)
        {
            this.Session.EnableFilter("MultitenantFilter").SetParameter("TenantId", tenantContext.ID);
        }
    }
}