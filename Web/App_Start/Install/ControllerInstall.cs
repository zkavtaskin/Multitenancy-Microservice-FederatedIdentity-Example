using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Server.Service;
using System.Web.Mvc;
using Castle.Facilities.TypedFactory;
using Server.Service.Tenants;
using Web.Middleware;

namespace Web.App_Start.Install
{
    public class ControllerInstall : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {

            container.Register(Component.For<Web.Filters.CustomHandleErrorAttribute>()
                .ImplementedBy<Web.Filters.CustomHandleErrorAttribute>().LifeStyle.Is(Castle.Core.LifestyleType.Singleton));

            container.Register(Classes.FromThisAssembly()
                           .BasedOn<IController>()
                           .Configure(c => c.LifeStyle.Is(Castle.Core.LifestyleType.Transient)));

            container.Register(Component.For<TenantContext>()
                .ImplementedBy<TenantContext>().LifeStyle.Is(Castle.Core.LifestyleType.PerWebRequest));

            container.AddFacility<TypedFactoryFacility>();

            container.Register(Component.For<ITenantContextFactory>().AsFactory());

            container.Register(Component.For<MultitenancyMiddleware<TenantDto>>()
                .ImplementedBy<MultitenancyMiddleware<TenantDto>>().LifeStyle.Is(Castle.Core.LifestyleType.PerWebRequest));

            container.Register(Component.For<ITenantNameExtractor>()
                .ImplementedBy<RouteDataTokensTenantNameExtractor>().LifeStyle.Is(Castle.Core.LifestyleType.PerWebRequest));

            container.Register(Component.For<TenantResolver>()
                .ImplementedBy<TenantResolver>().LifeStyle.Is(Castle.Core.LifestyleType.PerWebRequest));

            container.Register(Component.For<Middleware.ITenantResolver<TenantDto>>()
                .ImplementedBy<TenantResolverCacheDecorator>().LifeStyle.Is(Castle.Core.LifestyleType.PerWebRequest));

        }
    }
}