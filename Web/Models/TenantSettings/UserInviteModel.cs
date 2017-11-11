using System;

namespace Web.Models.TenantSettings
{
    public class UserInviteModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public int TotalHoursSinceInvite { get; set; }
    }
}