using FluentNHibernate.Mapping;
using NHibernate;

namespace Server.Core.Repository.Adapters.NHibernate
{
    public class MultitenantFilter : FilterDefinition
    {
        public MultitenantFilter()
        {
            WithName("MultitenantFilter").AddParameter("TenantId", NHibernateUtil.Guid);
        }
    }
}
