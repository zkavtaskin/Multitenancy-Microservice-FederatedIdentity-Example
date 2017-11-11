using System;

namespace Server.Service.Tenants
{
    public interface ITenantService
    {
        TenantDto Create(string tenantName, string friendlyName, string timeZoneId, string clientId, Uri authority, string userEmail);

        bool IsFriendlyNameAvailable(string name);

        string GenerateFriendlyName(string tenantName);

        TenantDto Get(string friendlyName);

        void ChangeTimeZone(Guid tenantId, string timeZoneId);
    }
}
