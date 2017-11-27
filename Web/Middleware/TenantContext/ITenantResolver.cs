using Server.Service.Tenants;

namespace Web.Middleware
{
    public interface ITenantResolver<TTenant>
    {
        TTenant GetTenant(string tenantName);
    }
}
