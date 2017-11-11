using Server.Core.Repository;
using System;

namespace Server.Domain.Tenants
{
    public class TenantDomainService
    {
        IRepository<Tenant> tenantRepo;

        public TenantDomainService(IRepository<Tenant> tenantRepo)
        {
            this.tenantRepo = tenantRepo;
        }

        public bool IsFriendlyNameAvailable(string name)
        {
             return this.tenantRepo.Count(x => x.NameFriendly.Contains(name)) == 0;
        }

        public string GenerateFriendlyName(string name)
        {
            return name.Trim().Replace(" ", "_").ToLower();
        }

        public void ChangeTimeZone(Guid tenantId, string timeZoneId)
        {
            Tenant tenant = this.tenantRepo.FindById(tenantId);
            tenant.ChangeTimeZone(timeZoneId);
        }
    }
}
