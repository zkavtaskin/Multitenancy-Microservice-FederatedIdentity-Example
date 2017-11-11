using System;

namespace Server.Service.Groups
{
    public class GroupUserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Initials { get; set; }

        public string PrimaryContact { get; set; }
    }
}
