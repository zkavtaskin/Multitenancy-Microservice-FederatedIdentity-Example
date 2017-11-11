using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Server.Core.Repository.Adapters.NHibernate
{
    public abstract class NHConfiguration
    {
        public abstract global::NHibernate.Cfg.Configuration Config { get; }
    }
}
