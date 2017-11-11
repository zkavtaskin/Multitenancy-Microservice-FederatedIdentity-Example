using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.TenantSettings
{
    public class TimeModel
    {
        public List<TimeZoneInfo> TimeZones { get; set; }

        [Required]
        [Display(Name = "Time Zone")]
        public string TimeZoneId { get; set; }
    }
}