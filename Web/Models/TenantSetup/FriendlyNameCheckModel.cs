using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class FriendlyNameCheckModel 
    {
        [StringLength(20, MinimumLength = 1)]
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
    }
}