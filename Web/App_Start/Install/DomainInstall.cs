using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Server.Domain.Groups;
using Server.Domain.Stops;
using Server.Domain.Tenants;
using Server.Domain.Users;

namespace Web.App_Start.Install
{
    public class DomainInstall : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyNamed("Server").InNamespace("Server.Domain")
                .BasedOn(typeof(Server.Core.Domain.Handles<>))
                .WithService.FromInterface(typeof(Server.Core.Domain.Handles<>))
                .Configure(c => c.LifeStyle.Is(Castle.Core.LifestyleType.Transient)));

            container.Register(Component.For<TenantDomainService>().ImplementedBy<TenantDomainService>()
                .LifeStyle.Is(Castle.Core.LifestyleType.Transient));

            container.Register(Component.For<UserDomainService>().ImplementedBy<UserDomainService>()
                .LifeStyle.Is(Castle.Core.LifestyleType.Transient));

            container.Register(Component.For<GroupDomainService>().ImplementedBy<GroupDomainService>()
                .LifeStyle.Is(Castle.Core.LifestyleType.Transient));

            container.Register(Component.For<StopDomainService>().ImplementedBy<StopDomainService>()
                .LifeStyle.Is(Castle.Core.LifestyleType.Transient));
        }
    }
}