using Microsoft.AspNet.SignalR;
using Web.Models.Group;

namespace Web.Hubs
{
    public class GroupHub : Hub
    {
        public void GroupLifeCycleStateChange(string tenantFriendlyName, LifeCycleState lifeCycleState)
        {
            Clients.All.groupLifeCycleStateChange(tenantFriendlyName, lifeCycleState);
        }
    }
}