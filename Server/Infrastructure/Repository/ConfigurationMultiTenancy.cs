using Server.Core.Repository.Adapters.NHibernate;

namespace Server.Infrastructure.Repository
{
    public class ConfigurationMultiTenancy : Configuration
    {
        public ConfigurationMultiTenancy(NHSharedDatabaseMultiTenancyInterceptor multitenancy)
        {
            this.Config.SetInterceptor(multitenancy);
        }
    }
}