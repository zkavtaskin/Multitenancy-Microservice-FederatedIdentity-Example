using System;

namespace Web.Models.Group
{
    public class StopModel
    {
        public string By { get; set; }
        public DateTime Date { get; set; }
        public Nullable<TimeSpan> OverallDownTime { get; set; }
        public string Problem { get; set; }
    }
}