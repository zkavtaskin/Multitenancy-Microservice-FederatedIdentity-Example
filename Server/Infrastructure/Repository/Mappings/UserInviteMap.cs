using FluentNHibernate.Mapping;
using Server.Domain.Users;
using FluentNHibernate;
using Server.Core.Repository.Adapters.NHibernate;

namespace Server.Infrastructure.Repository.Mappings
{
    public class UserInviteMap : ClassMap<UserInvite>
    {
        public UserInviteMap()
        {
            Id(p => p.Id).GeneratedBy.Assigned();
            Map(p => p.Email).Not.Nullable();
            Map(p => p.DateCreated).Not.Nullable();
            Map(p => p.DateModified).Not.Nullable();
            Map(p => p.InviteKey).Not.Nullable();
            Map(p => p.InvitedByUserId).Not.Nullable();

            HasMany(p => p.GroupIds)
                .Access
                .CamelCaseField()
                .Table("UserInviteGroup")
                .KeyColumn("UserInviteId")
                .Element("GroupId")
                .Cascade.AllDeleteOrphan()
                .LazyLoad();

            Map(Reveal.Member<UserInvite>("tenantId")).Not.Nullable();
            ApplyFilter<MultitenantFilter>("TenantId = :TenantId");
        }
    }
}
