using AutoMapper;
using Server.Domain.Groups;
using Server.Domain.Users;
using System.Linq;

namespace Server.Service.Groups
{
    internal class Map : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Group, GroupDto>();

            Mapper.CreateMap<User, GroupUserDto>()
                .ForMember(x => x.PrimaryContact, 
                opt => opt.MapFrom(x => 
                     x.Contacts.FirstOrDefault().Value));

            Mapper.CreateMap<UserInvite, GroupInvitedUserDto>();
        }
    }
}
