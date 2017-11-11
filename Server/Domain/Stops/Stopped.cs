using Server.Core.Domain;

namespace Server.Domain.Stops
{
    public class Stopped : IDomainEvent
    {
        public Stop Stop { get; protected set; }

        public Stopped(Stop stop)
        {
            this.Stop = stop;
        }
    }
}
