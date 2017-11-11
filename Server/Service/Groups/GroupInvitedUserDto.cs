using System;

namespace Server.Service.Groups
{
    public class GroupInvitedUserDto
    {
        public string Email { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid Id { get; set; }
    }
}
