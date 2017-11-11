using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Dashboard
{
    public class StopModel
    {
        public Guid GroupId { get; set; }

        [Required(ErrorMessage = "Reason for stopping is required")]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "Reason for stopping (6-150 Characters)")]
        public string Reason { get; set; }
    }
}