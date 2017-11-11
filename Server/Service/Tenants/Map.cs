using AutoMapper;
using Server.Domain.Tenants;

namespace Server.Service.Tenants
{
    internal class Map : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Tenant, TenantDto>()
                .ForMember(x => x.AuthAuthority, options => options.MapFrom(x => x.Authority))
                .ForMember(x => x.AuthClientId, options => options.MapFrom(x => x.ClientId));
        }
    }
}
