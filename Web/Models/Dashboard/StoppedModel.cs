using System.Collections.Generic;

namespace Web.Models.Dashboard
{
    public class UserStoppedModel : Model
    {
        public UserStoppedModel(string tenantFriendlyName)
            : base(tenantFriendlyName)
        {
            
        }

        public GroupModel Group { get; set; }

        public List<Web.Models.Group.UserModel> Users { get; set; }
    }
}