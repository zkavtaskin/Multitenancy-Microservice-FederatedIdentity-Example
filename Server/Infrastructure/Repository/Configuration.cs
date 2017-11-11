using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;
using Server.Core.Repository.Adapters.NHibernate;

namespace Server.Infrastructure.Repository
{
    public class Configuration : NHConfiguration
    {
        public override NHibernate.Cfg.Configuration Config => config;

        private NHibernate.Cfg.Configuration config;

        public Configuration()
        {
            this.config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(c => c.FromConnectionStringWithKey("default"))
#if DEBUG
                    .ShowSql()
                    .FormatSql()
#endif
                    .AdoNetBatchSize(50)
                ).Mappings(m =>
                    m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())
                )
                .ExposeConfiguration(c => SchemaMetadataUpdater.QuoteTableAndColumns(c))
                .BuildConfiguration();
        }
    }
}