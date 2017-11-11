using System.ComponentModel.DataAnnotations;

namespace Web.Models.Group
{
    public class CreateModel
    {
        [Required]
        [StringLength(30, MinimumLength = 1)]
        [Display(Name = "Production line name")]

        public string Name { get; set; }

        [Display(Name = "Team Members")]
        public string Invite { get; set; }

        public string Invited { get; set; }

        public bool NameIsAvailable { get; set; }

        public CreateModel()
        {
            this.NameIsAvailable = true;
        }
    }
}