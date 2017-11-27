using System;
using Microsoft.Owin;
using System.Threading.Tasks;
using Server.Service;
using Server.Service.Tenants;
using Web.Extensions;

namespace Web.Middleware
{
    public class MultitenancyMiddleware
    {
        readonly ITenantNameExtractor tenantNameExtractor;
        readonly ITenantResolver tenantResolver;
        readonly MultitenancyNotifications notifications;
        readonly Func<Task> next;

        public MultitenancyMiddleware(Func<Task> next, ITenantNameExtractor tenantNameExtractor, 
            ITenantResolver tenantResolver,  MultitenancyNotifications notifications)
        {
            this.tenantNameExtractor = tenantNameExtractor;
            this.tenantResolver = tenantResolver;
            this.notifications = notifications;
            this.next = next;
        }

        public async Task Invoke(IOwinContext context)
        {
            if (!this.tenantNameExtractor.CanExtract(context))
            {
                await this.next();
                return;
            }

            string name = this.tenantNameExtractor.GetName(context);

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

            TenantContext tenantContext = await this.notifications.TenantDataResolved(context, tenant);

            context.SetTenantContext(tenantContext);

            await this.next();
        }
    }
}