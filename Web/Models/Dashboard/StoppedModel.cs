using System.Collections.Generic;

namespace Web.Models.Dashboard
{
    public class UserStoppedModel : Model
    {
        public GroupModel Group { get; set; }

        public List<Web.Models.Group.UserModel> Users { get; set; }
    }
}