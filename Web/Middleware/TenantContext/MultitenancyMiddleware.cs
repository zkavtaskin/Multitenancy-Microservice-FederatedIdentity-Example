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
        readonly ITenantIdentifierExtractor tenantIdentifierExtractor;
        readonly ITenantRecordResolver<TTenantRecord> tenantRecordResolver;
        readonly MultitenancyNotifications<TTenantRecord> notifications;
        readonly Func<Task> next;

        public MultitenancyMiddleware(Func<Task> next, ITenantIdentifierExtractor tenantIdentifierExtractor, 
            ITenantRecordResolver<TTenantRecord> tenantRecordResolver,  MultitenancyNotifications<TTenantRecord> notifications)
        {
            this.next = next;
            this.tenantIdentifierExtractor = tenantIdentifierExtractor;
            this.tenantRecordResolver = tenantRecordResolver;
            this.notifications = notifications;
        }

        public async Task Invoke(IOwinContext context)
        {
            if (!this.tenantIdentifierExtractor.CanExtract(context))
            {
                await this.next();
                return;
            }

            string identifier = this.tenantIdentifierExtractor.GetIdentifier(context);

            if (string.IsNullOrEmpty(identifier))
            {
                await this.notifications.TenantIdentifierNotFound(context);
                return;
            }

            TTenantRecord tenantRecord = this.tenantRecordResolver.GetTenant(identifier);
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