using System.Collections.Generic;

namespace Web.Models.Dashboard
{
    public class GroupsStateModel : Model
    {
        public GroupsStateModel(string tenantFriendlyName)
            : base(tenantFriendlyName)
        {
            
        }
        public List<GroupModel> Groups { get; set; }
    }
}