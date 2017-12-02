using System;
using System.Reflection;
using NHibernate;
using NHibernate.Type;
using Server.Service;

namespace Server.Core.Repository.Adapters.NHibernate
{
    public class NHSharedDatabaseMultiTenancyInterceptor : EmptyInterceptor
    {
        readonly TenantContext tenantContext;

        public NHSharedDatabaseMultiTenancyInterceptor(TenantContext tenantContext)
        {
            this.tenantContext = tenantContext;
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            int index = Array.IndexOf(propertyNames, "tenantId");

            if (index == -1)
                return false;

            state[index] = this.tenantContext.ID;

            entity.GetType()
                  .GetField("tenantId", BindingFlags.Instance | BindingFlags.NonPublic)
                  .SetValue(entity, tenantContext.ID);

            return base.OnSave(entity, id, state, propertyNames, types);
        }
    }
}
