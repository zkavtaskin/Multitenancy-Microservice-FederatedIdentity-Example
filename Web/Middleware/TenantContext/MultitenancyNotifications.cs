using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Server.Service;
using Server.Service.Tenants;

namespace Web.Middleware
{
    public class MultitenancyNotifications<TTenantRecord>
        where TTenantRecord : class
    {
        public Func<IOwinContext, Task> TenantIdentifierNotFound { get; set; }
        public Func<IOwinContext, Task> TenantRecordNotFound { get; set; }
        public Func<IOwinContext, TTenantRecord, Task<TenantContext>> CreateTenantContext { get; set; }

        public MultitenancyNotifications()
        {
            this.TenantIdentifierNotFound = context => Task.FromResult(0);
            this.TenantRecordNotFound = context => Task.FromResult(0);
            this.CreateTenantContext = (context, tenantRecord) => Task.FromResult<TenantContext>(null);
        }
    }
}