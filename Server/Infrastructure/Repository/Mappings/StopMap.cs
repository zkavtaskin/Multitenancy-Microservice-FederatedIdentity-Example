using FluentNHibernate.Mapping;
using Server.Domain.Stops;
using FluentNHibernate;
using Server.Core.Repository.Adapters.NHibernate;

namespace Server.Infrastructure.Repository.Mappings
{
    public class StopMap : ClassMap<Stop>
    {
        public StopMap()
        {
            Id(p => p.Id).GeneratedBy.Assigned();
            Map(p => p.Problem).Not.Nullable();
            Map(p => p.By).Not.Nullable();
            Map(p => p.ById).Not.Nullable();
            Map(p => p.Date).Not.Nullable();
            Map(p => p.WhenResolved);
            Map(p => p.GroupName).Not.Nullable();
            Map(p => p.GroupId).Not.Nullable();
            HasMany(p => p.GroupUsers)
                .Access.CamelCaseField()
                .Table("StopGroupUser")
                .KeyColumn("StopId")
                .Element("UserEmail")
                .Cascade.All().LazyLoad();

            Map(Reveal.Member<Stop>("tenantId")).Not.Nullable();
            ApplyFilter<MultitenantFilter>("TenantId = :TenantId");
        }
    }
}
