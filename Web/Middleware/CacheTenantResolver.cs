using System;
using System.Runtime.Caching;
using Server.Service.Tenants;

namespace Web.Middleware
{
    public class CacheTenantResolver : ITenantResolver
    {
        private TenantResolver tenantResolver;

        public CacheTenantResolver(TenantResolver tenantResolver)
        {
            this.tenantResolver = tenantResolver;
        }

        public TenantDto GetTenant(string tenantName)
        {
            string cacheKey = $"tenantName:{tenantName}";

            TenantDto tenant = (TenantDto)MemoryCache.Default[cacheKey];

            if (tenant != null)
                return tenant;

            tenant = this.tenantResolver.GetTenant(tenantName);
            if (tenant == null)
                return null;

            MemoryCache.Default.Set(cacheKey, tenant, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
            });
       
            return tenant;
        }
    }
}