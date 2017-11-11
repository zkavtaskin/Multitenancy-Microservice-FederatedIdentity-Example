using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Server.Service;

namespace Server.Core.Repository.Adapters.NHibernate
{
    public class NHUnitOfWorkMultitenancy : NHUnitOfWork
    {
        public NHUnitOfWorkMultitenancy(NHConfiguration nhConfiguration, TenantContext tenantContext) 
            : base(nhConfiguration)
        {
            this.Session.EnableFilter("MultitenantFilter").SetParameter("TenantId", tenantContext.ID);
        }
    }
}