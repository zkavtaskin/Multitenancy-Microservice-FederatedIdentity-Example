using AutoMapper;
using Server.Core.Time;
using Server.Service.Groups;
using Server.Service.Stops;
using Server.Service.Users;

namespace Web.Models
{
    internal class Map : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<UserDto, Web.Models.TenantSettings.UserModel>();

            Mapper.CreateMap<UserInviteDto, Web.Models.TenantSettings.UserInviteModel>()
                .ForMember(x => x.TotalHoursSinceInvite, 
                opt => opt.MapFrom(
                    x => (TimeProvider.Current.UtcNow - x.DateModified).TotalHours));

            Mapper.CreateMap<StopDto, Web.Models.GroupModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.GroupName))
                .ForMember(x => x.DownTime, opt => opt.MapFrom(x => TimeProvider.Current.UtcNow - x.Date))
                .ForMember(x => x.StoppedBy, opt => opt.MapFrom(x => x.By))
                .ForMember(x => x.State, opt => opt.UseValue(Web.Models.Group.State.Stopped))
                .ForMember(x => x.StopId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.StoppedDateTime, opt => opt.MapFrom(x => x.Date));

            Mapper.CreateMap<StopDto, Web.Models.Group.StopModel>();

            Mapper.CreateMap<GroupUserDto, Web.Models.Group.UserModel>();

            Mapper.CreateMap<GroupDto, Web.Models.Group.Model>()
                .ForMember(x => x.NumberOfMembers, opt => opt.MapFrom(x => x.Users.Count))
                .ForMember(x => x.State, opt => opt.UseValue(Web.Models.Group.State.Working));

            Mapper.CreateMap<GroupUserDto, Web.Models.Group.UserModel>();
            Mapper.CreateMap<GroupInvitedUserDto, Web.Models.Group.UserModel>()
                .ForMember(x => x.IsInvited, opt => opt.UseValue(true));


            Mapper.CreateMap<UserDto, Web.Models.Group.UserSuggestionModel>();

            Mapper.CreateMap<UserInviteDto, Web.Models.Group.UserSuggestionModel>()
                .ForMember(x => x.IsInvited, opt => opt.UseValue(true));
        }
    }
}
