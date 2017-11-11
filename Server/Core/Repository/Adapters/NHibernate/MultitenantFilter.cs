using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using NHibernate;

namespace Server.Core.Repository.Adapters.NHibernate
{
    public class MultitenantFilter : FilterDefinition
    {
        public MultitenantFilter()
        {
            WithName("MultitenantFilter").AddParameter("TenantId", NHibernateUtil.Guid);
        }
    }
}
