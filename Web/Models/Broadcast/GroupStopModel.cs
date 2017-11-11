using System;

namespace Web.Models.Broadcast
{
    [Obsolete]
    public class GroupStopModel
    {
        public Guid StopId { get; set; }
        public Guid GroupId { get; set; }
        public string Name { get; set; }

        public string StoppedBy { get; set; }
        public Nullable<TimeSpan> DownTime { get; set; }

        public DateTime StoppedDateTime { get; set; }

    }
}