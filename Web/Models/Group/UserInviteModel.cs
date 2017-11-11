using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Group
{
    public class UserInviteModel
    {
        [Required]
        public Guid GroupId { get; set; }

        [Required]
        public string Email { get; set; }
    }
}