using Server.Service.Tenants;

namespace Web.Middleware
{
    public class TenantResolver : ITenantResolver
    {
        readonly ITenantService tenantService;

        public TenantResolver(ITenantService tenantService)
        {
            this.tenantService = tenantService;
        }

        public TenantDto GetTenant(string tenantName)
        {
            return this.tenantService.Get(tenantName);
        }
    }
}