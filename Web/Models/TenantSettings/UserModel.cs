using System;

namespace Web.Models.TenantSettings
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public string Initials { get; set; }

        public bool IsManager { get; set; }

        public bool CanRemove { get; set; }
    }
}