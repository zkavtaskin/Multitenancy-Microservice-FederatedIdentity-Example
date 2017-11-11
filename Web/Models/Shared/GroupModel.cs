using System;

namespace Web.Models
{
    public class GroupModel
    {
        public Guid StopId { get; set; }
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public Group.State State { get; set; }
        
        public string StoppedBy { get; set; }
        public Nullable<TimeSpan> DownTime { get; set; }

        public DateTime StoppedDateTime { get; set; }

        public bool CanStop { get; set; }
    }
}