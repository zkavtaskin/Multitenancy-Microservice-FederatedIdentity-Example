using System;
using System.Collections.Generic;

namespace Server.Service.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Initials { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public List<ContactDto> Contacts { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
