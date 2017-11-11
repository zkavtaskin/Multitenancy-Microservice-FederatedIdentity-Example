using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Server.Service;
using Server.Service.Tenants;

namespace Web.Middleware
{
    public class MultitenancyNotifications
    {
        public Func<IOwinContext, Task> TenantCouldNotBeResolved { get; set; }
        public Func<IOwinContext, Task> TenantNameCouldNotBeFound { get; set; }
        public Func<IOwinContext, ITenantContextFactory, TenantDto, Task> TenantResolved { get; set; }

        public MultitenancyNotifications()
        {
            this.TenantCouldNotBeResolved = context => Task.FromResult(0);
            this.TenantNameCouldNotBeFound = context => Task.FromResult(0);
            this.TenantResolved = (context, tenantContextFactory, tenantDto) => Task.FromResult(0);
        }
    }
}