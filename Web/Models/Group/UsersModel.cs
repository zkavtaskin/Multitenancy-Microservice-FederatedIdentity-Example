using System;
using System.Collections.Generic;

namespace Web.Models.Group
{
    public class UsersModel 
    {
        public UsersModel()
        {
            this.Users = new List<UserModel>();
            this.MonthStops = new List<StopSumModel>();
        }

        public Guid GroupId { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public List<UserModel> Users { get; set; }

        public List<StopSumModel> MonthStops { get; set; }
    }
}