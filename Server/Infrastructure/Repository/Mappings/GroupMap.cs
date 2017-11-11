using FluentNHibernate.Mapping;
using Server.Domain.Groups;
using FluentNHibernate;
using Server.Core.Repository.Adapters.NHibernate;

namespace Server.Infrastructure.Repository.Mappings
{
    public class GroupMap : ClassMap<Group>
    {
        public GroupMap()
        {
            Id(p => p.Id).GeneratedBy.Assigned();
            Map(p => p.Name).Not.Nullable();

            HasManyToMany(p => p.Users)
                .Access
                .CamelCaseField()
                .Table("GroupUser")
                .ParentKeyColumn("GroupId")
                .ChildKeyColumn("UserId")
                .LazyLoad();

            HasManyToMany(p => p.Invites)
                .Access
                .CamelCaseField()
                .Table("UserInviteGroup")
                .ParentKeyColumn("GroupId")
                .ChildKeyColumn("UserInviteId")
                .LazyLoad();

            Map(p => p.DateCreated).Not.Nullable();
            Map(p => p.DateModified).Not.Nullable();

            Map(Reveal.Member<Group>("tenantId")).Not.Nullable();
            ApplyFilter<MultitenantFilter>("TenantId = :TenantId");
        }
    }
}
