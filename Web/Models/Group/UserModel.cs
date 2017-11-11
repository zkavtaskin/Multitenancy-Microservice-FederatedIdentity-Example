using System;

namespace Web.Models.Group
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Initials { get; set; }
        public string FullName { get; set; }
        public bool IsInvited { get; set; }
        public string PrimaryContact { get; set; }

        public Guid GroupId { get; set; }

        public UserModel()
        {

        }
    }
}