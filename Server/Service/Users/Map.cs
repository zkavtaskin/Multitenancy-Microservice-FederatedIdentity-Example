using AutoMapper;
using Server.Domain.Users;

namespace Server.Service.Users
{
    internal class Map : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<User, UserDto>();
            Mapper.CreateMap<UserContact, ContactDto>();
            Mapper.CreateMap<UserInvite, UserInviteDto>();

        }
    }
}
