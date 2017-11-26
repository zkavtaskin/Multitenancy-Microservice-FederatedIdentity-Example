using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Server.Service;
using Server.Service.Tenants;

namespace Web.Middleware
{
    public class MultitenancyNotifications
    {
        public Func<IOwinContext, Task> TenantDataCouldNotBeResolved { get; set; }
        public Func<IOwinContext, Task> TenantNameCouldNotBeFound { get; set; }
        public Func<IOwinContext, TenantDto, Task<TenantContext>> TenantDataResolved { get; set; }

        public MultitenancyNotifications()
        {
            this.TenantDataCouldNotBeResolved = context => Task.FromResult(0);
            this.TenantNameCouldNotBeFound = context => Task.FromResult(0);
            this.TenantDataResolved = (context, tenantDto) => Task.FromResult<TenantContext>(null);
        }
    }
}