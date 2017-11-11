using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class TenantNameModel 
    {
        [StringLength(20, MinimumLength = 1)]
        [Display(Name = "Tenant Name")]
        [RegularExpression(@"^[A-Za-z0-9 ]{1,30}$", ErrorMessage = "Tenant name can't contain special characters such as &*^&")]
        public string Name { get; set; }

        public string FriendlyName { get; set; }
    }
}