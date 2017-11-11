using Server.Core.Domain;

namespace Server.Domain.Stops
{
    public class Resumed : IDomainEvent
    {
        public Stop Stop { get; protected set; }

        public Resumed(Stop stop)
        {
            this.Stop = stop;
        }
    }
}
