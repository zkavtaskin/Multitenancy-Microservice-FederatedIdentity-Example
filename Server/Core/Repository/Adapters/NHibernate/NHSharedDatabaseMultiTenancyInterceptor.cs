using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Type;
using Server.Service;

namespace Server.Core.Repository.Adapters.NHibernate
{
    public class NHSharedDatabaseMultiTenancyInterceptor : EmptyInterceptor
    {
        private readonly TenantContext tenantContext;

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
