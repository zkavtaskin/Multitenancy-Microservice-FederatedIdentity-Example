using System;
using System.Runtime.Caching;
using Server.Service.Tenants;

namespace Web.Middleware
{
    public class TenantRecordResolverCacheDecorator : ITenantRecordResolver<TenantDto>
    {
        readonly TenantRecordResolver tenantRecordResolver;

        public TenantRecordResolverCacheDecorator(TenantRecordResolver tenantRecordResolver)
        {
            this.tenantRecordResolver = tenantRecordResolver;
        } 

        public TenantDto GetTenant(string tenantIdentifier)
        {
            string cacheKey = $"tenantIdentifier:{tenantIdentifier}";

            TenantDto tenant = (TenantDto)MemoryCache.Default[cacheKey];

            if (tenant != null)
            {
                return tenant;
            }

            tenant = this.tenantRecordResolver.GetTenant(tenantIdentifier);
            if (tenant == null)
            {
                return null;
            }

            MemoryCache.Default.Set(cacheKey, tenant, new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(10)
            });
       
            return tenant;
        }
    }
}