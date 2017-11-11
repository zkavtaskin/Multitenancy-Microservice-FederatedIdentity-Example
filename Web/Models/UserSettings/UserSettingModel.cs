using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class UserSettingModel
    {
        [Required]
        [StringLength(70, MinimumLength = 6)]
        [Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.']+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Email address is invalid")]
        public string Email { get; set; }

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

        public bool EmailIsAlreadyUsed { get; set; }
    }
}