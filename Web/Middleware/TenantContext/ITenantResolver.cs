using Server.Service.Tenants;

namespace Web.Middleware
{
    public interface ITenantResolver
    {
        TenantDto GetTenant(string tenantName);
    }
}
