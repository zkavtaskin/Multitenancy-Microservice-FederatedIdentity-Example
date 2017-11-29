using Server.Service.Tenants;

namespace Web.Middleware
{
    public class TenantRecordResolver : ITenantRecordResolver<TenantDto>
    {
        readonly ITenantService tenantService;

        public TenantRecordResolver(ITenantService tenantService)
        {
            this.tenantService = tenantService;
        }

        public TenantDto GetTenant(string tenantIdentifier)
        {
            return this.tenantService.Get(tenantIdentifier);
        }
    }
}