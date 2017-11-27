using System;
using Microsoft.Owin;
using System.Threading.Tasks;
using Server.Service;
using Server.Service.Tenants;
using Web.Extensions;

namespace Web.Middleware
{
    public class MultitenancyMiddleware<TTenantRecord>
        where TTenantRecord : class
    {
        readonly ITenantNameExtractor tenantNameExtractor;
        readonly ITenantResolver<TTenantRecord> tenantResolver;
        readonly MultitenancyNotifications<TTenantRecord> notifications;
        readonly Func<Task> next;

        public MultitenancyMiddleware(Func<Task> next, ITenantNameExtractor tenantNameExtractor, 
            ITenantResolver<TTenantRecord> tenantResolver,  MultitenancyNotifications<TTenantRecord> notifications)
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
                await this.notifications.TenantNameNotFound(context);
                return;
            }

            TTenantRecord tenantRecord = this.tenantResolver.GetTenant(name);
            if (tenantRecord == null)
            {
                await this.notifications.TenantRecordNotFound(context);
                return;
            }

            TenantContext tenantContext = await this.notifications.CreateTenantContext(context, tenantRecord);

            context.SetTenantContext(tenantContext);

            await this.next();
        }
    }
}