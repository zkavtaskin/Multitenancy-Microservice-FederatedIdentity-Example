namespace Server.Core.Repository.Adapters.NHibernate
{
    public abstract class NHConfiguration
    {
        public abstract global::NHibernate.Cfg.Configuration Config { get; }
    }
}
