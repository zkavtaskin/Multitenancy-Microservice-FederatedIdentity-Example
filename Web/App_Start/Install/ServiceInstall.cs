using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using log4net;
using Server.Core.ConfigManager;
using Server.Service.Groups;
using Server.Service.Stops;
using Server.Service.Tenants;
using Server.Service.Users;
using Server.Core.Repository.Adapters.NHibernate;
using Server.Core.Repository;

namespace Web.App_Start.Install
{
    public class ServiceInstall : IWindsorInstaller
    {
        ILog logger;

        public ServiceInstall(ILog logger)
        {
            this.logger = logger;
        }

        public void Install(IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Component.For<ITenantService>()
                .ImplementedBy<TenantService>()
                .LifeStyle.Is(LifestyleType.Transient));

            container.Register(Component.For<IGroupService>()
                .ImplementedBy<GroupService>()
                .DependsOn(Dependency.OnComponent<IUnitOfWork, NHUnitOfWorkMultitenancy>())
                .LifeStyle.Is(LifestyleType.Transient));

            container.Register(Component.For<IStopService>()
                .ImplementedBy<StopService>()
                .DependsOn(Dependency.OnComponent<IUnitOfWork, NHUnitOfWorkMultitenancy>())
                .LifeStyle.Is(LifestyleType.Transient));

            container.Register(Component.For<IUserService>()
                .ImplementedBy<UserService>()
                .DependsOn(Dependency.OnComponent<IUnitOfWork, NHUnitOfWorkMultitenancy>())
                .LifeStyle.Is(LifestyleType.Transient));

            container.Register(Component.For<IConfigurationProvider>().ImplementedBy<CloudConfigurationProvider>());

            container.Register(Component.For<ILog>().Instance(this.logger)
                .LifeStyle.Is(Castle.Core.LifestyleType.Singleton));
        }
    }
}