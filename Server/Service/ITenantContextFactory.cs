using System;

namespace Server.Service
{
    public interface ITenantContextFactory
    {
        TenantContext Create(Guid id, string friendlyName, string authClientId, string authAuthority);
    }
}
