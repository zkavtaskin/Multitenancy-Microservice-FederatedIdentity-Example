using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class UserSetupModel 
    {
        [Required]
        [StringLength(35, MinimumLength = 2)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [StringLength(18, MinimumLength = 6)]
        [Display(Name = "Primary phone number")]
        public string PrimaryNumber { get; set; }

        [StringLength(18, MinimumLength = 6)]
        [Display(Name = "Secondary phone number")]
        public string SecondaryNumber { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public Guid Key { get; set; }
    }
}