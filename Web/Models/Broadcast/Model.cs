using System.Collections.Generic;

namespace Web.Models.Broadcast
{
    public class Model
    { 
        public string TenantFriendlyName { get; }

        public List<GroupModel> Groups { get; set; }

        public Model(string tenantFriendlyName)
        {
            this.TenantFriendlyName = tenantFriendlyName;
        }
    }
}