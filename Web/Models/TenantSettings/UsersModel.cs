using System.Collections.Generic;

namespace Web.Models.TenantSettings
{
    public class UsersModel
    {
        public TimeModel Time { get; set; }

        public List<UserModel> Users { get; set; }
        public List<UserInviteModel> UserInvites { get; set; }
    }
}