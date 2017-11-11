using AutoMapper;

namespace Server.Service
{
    public static class MappingConfig
    {
        public static void RegisterMapping()
        {
            Mapper.AddProfile(new Users.Map());
            Mapper.AddProfile(new Groups.Map());
            Mapper.AddProfile(new Stops.Map());
            Mapper.AddProfile(new Tenants.Map());
        }
    }
}
