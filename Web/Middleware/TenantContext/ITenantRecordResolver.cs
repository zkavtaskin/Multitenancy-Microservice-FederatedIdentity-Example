using Server.Service.Tenants;

namespace Web.Middleware
{
    public interface ITenantRecordResolver<TTenantRecord>
    {
        TTenantRecord GetTenant(string tenantIdentifier);
    }
}
