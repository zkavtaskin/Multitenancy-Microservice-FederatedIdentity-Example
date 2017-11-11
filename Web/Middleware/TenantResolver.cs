using Server.Service.Tenants;

namespace Web.Middleware
{
    public class TenantResolver : ITenantResolver
    {
        private readonly ITenantService tenantService;

        public TenantResolver(ITenantService tenantService)
        {
            this.tenantService = tenantService;
        }

        public TenantDto GetTenant(string tenantName)
        {
            TenantDto tenantDto = this.tenantService.Get(tenantName);
            if (tenantDto != null)
            {
                return tenantDto;
            }
            return null;
        }
    }
}