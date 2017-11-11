using FluentNHibernate.Mapping;
using Server.Domain.Users;
using FluentNHibernate;
using Server.Core.Repository.Adapters.NHibernate;

namespace Server.Infrastructure.Repository.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(p => p.Id).GeneratedBy.Assigned();
            Map(p => p.IDPID).Not.Nullable();
            Map(p => p.FullName).Not.Nullable();
            Map(p => p.Email).Not.Nullable();
            Map(p => p.IsAdmin).Not.Nullable();
            HasMany(p => p.Contacts)
                .Access
                .CamelCaseField()
                .Table("UserContact")
                .KeyColumn("UserId")
                .Cascade.AllDeleteOrphan().Inverse().LazyLoad();

            Map(p => p.DateCreated).Not.Nullable();
            Map(p => p.DateModified).Not.Nullable();

            Map(Reveal.Member<User>("tenantId")).Not.Nullable();
            ApplyFilter<MultitenantFilter>("TenantId = :TenantId");
        }
    }
}
