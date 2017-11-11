using System;
using System.Collections.Generic;

namespace Web.Models.Group
{
    public class StopSumModel
    {
        public DateTime Date { get; set; }

        public List<StopModel> Stops { get; set; }

        public TimeSpan TotalDownTime { get; set; }
    }
}