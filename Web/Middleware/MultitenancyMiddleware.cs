using System;
using Microsoft.Owin;
using System.Threading.Tasks;
using Server.Service;
using Server.Service.Tenants;

namespace Web.Middleware
{
    public class MultitenancyMiddleware
    {
        ITenantNameExtractor tenantNameExtractor;
        ITenantResolver tenantResolver;
        ITenantContextFactory tenantContextFactory;
        MultitenancyNotifications notifications;

        public MultitenancyMiddleware(ITenantNameExtractor tenantNameExtractor, 
            ITenantResolver tenantResolver, ITenantContextFactory tenantContextFactory, MultitenancyNotifications notifications)
        {
            this.tenantNameExtractor = tenantNameExtractor;
            this.tenantResolver = tenantResolver;
            this.tenantContextFactory = tenantContextFactory;
            this.notifications = notifications;
        }

        public async Task Invoke(IOwinContext context, Func<Task> next)
        {
            string name = this.tenantNameExtractor.GetName();

            if (string.IsNullOrEmpty(name))
            {
                await this.notifications.TenantNameCouldNotBeFound(context);
                return;
            }

            TenantDto tenant = this.tenantResolver.GetTenant(name);
            if (tenant == null)
            {
                await this.notifications.TenantDataCouldNotBeResolved(context);
                return;
            }

            await this.notifications.TenantDataResolved(context, this.tenantContextFactory, tenant);
            await next();
        }
    }
}