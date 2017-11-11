using FluentNHibernate.Mapping;
using Server.Domain.Users;

namespace Server.Infrastructure.Repository.Mappings
{
    public class UserContactMap : ClassMap<UserContact>
    {
        public UserContactMap()
        {
            Id(p => p.Id).GeneratedBy.Assigned();
            Map(p => p.UserId).Not.Nullable();
            Map(p => p.Type).Not.Nullable();
            Map(p => p.Value).Not.Nullable();
        }
    }
}
