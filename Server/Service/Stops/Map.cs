using AutoMapper;
using Server.Domain.Stops;

namespace Server.Service.Stops
{
    internal class Map : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Stop, StopDto>();
        }
    }
}
