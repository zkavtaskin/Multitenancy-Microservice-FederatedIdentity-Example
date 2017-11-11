using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class TenantCreateModel
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        [Display(Name = "Organisation Name")]
        [RegularExpression(@"^[A-Za-z0-9 ]{1,30}$", ErrorMessage = "Organisation name can't contain special characters such as &*^&")]
        public string TenantName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        [Display(Name = "Organisation Friendly Name")]
        [RegularExpression(@"^[a-z0-9_]{1,30}$", ErrorMessage = "Organisation friendly name can't contain special characters or spaces")]
        public string TenantFriendlyName { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 6)]
        [Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.']+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email address is invalid")]
        public string UserEmail { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        [Display(Name = "Client ID")]
        public string ClientID { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        [Display(Name = "Authority")]
        public string Authority { get; set; }

        public List<TimeZoneInfo> TimeZones { get; set; }

        [Required]
        [Display(Name = "Time Zone")]
        public string TimeZoneId { get; set; }
    }
}