
using System;
using System.Collections.Generic;

namespace Server.Service.Groups
{
    public class GroupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<GroupUserDto> Users { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
