
using System;

namespace Server.Service.Users
{
    public class UserInviteDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}
