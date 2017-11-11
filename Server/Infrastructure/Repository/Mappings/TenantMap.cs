using FluentNHibernate.Mapping;
using Server.Domain.Tenants;

namespace Server.Infrastructure.Repository.Mappings
{
    public class TenantMap : ClassMap<Tenant>
    {
        public  TenantMap()
        {
            Id(m => m.Id).GeneratedBy.Assigned();
            Map(m => m.Name).Not.Nullable();
            Map(m => m.NameFriendly).Not.Nullable();
            Map(m => m.Authority).Not.Nullable();
            Map(m => m.ClientId).Not.Nullable();
            Map(m => m.TimeZoneId).Not.Nullable();
            Map(m => m.DateCreated).Not.Nullable();
            Map(m => m.DateModified).Not.Nullable();
        }
    }
}
