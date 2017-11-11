using System;
using System.Collections.Generic;

namespace Server.Service.Stops
{
    public class StopDto
    {
        public Guid Id { get; set; }
        public string Problem { get; set; }
        public string By { get; set; }
        public Guid ById { get; set; }
        public DateTime Date { get; set; }
        public Nullable<DateTime> WhenResolved { get; set; }
        public string GroupName { get; set; }
        public Guid GroupId { get; set; }
        public List<string> GroupUsers { get; set; }
        public Nullable<TimeSpan> OverallDownTime { get; set; }
    }
}
