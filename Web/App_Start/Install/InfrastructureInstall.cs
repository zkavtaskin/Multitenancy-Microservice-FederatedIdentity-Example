using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Server.Core.Message;
using Server.Core.Message.Email;
using Server.Core.Repository;
using Server.Core.Repository.Adapters.NHibernate;
using Server.Domain.Tenants;
using Server.Infrastructure.Repository;

namespace Web.App_Start.Install
{
    public class InfrastructureInstall : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Component.For<NHConfiguration>()
                .ImplementedBy<Configuration>().LifeStyle.Is(LifestyleType.Singleton));

            container.Register(Component.For<IUnitOfWork>()
                .ImplementedBy<NHUnitOfWork>()
                .Forward<NHUnitOfWork>()
                .DependsOn(Dependency.OnComponent<NHConfiguration, Configuration>())
                .LifeStyle.Is(LifestyleType.PerWebRequest));

            container.Register(Component.For<IRepository<Tenant>>()
                .ImplementedBy<NHRepository<Tenant>>()
                .DependsOn(Dependency.OnComponent<IUnitOfWork, NHUnitOfWork>())
                .LifeStyle.Is(LifestyleType.PerWebRequest));


            container.Register(Component.For<NHSharedDatabaseMultiTenancyInterceptor>()
                .ImplementedBy<NHSharedDatabaseMultiTenancyInterceptor>().LifeStyle.Is(LifestyleType.PerWebRequest));

            container.Register(Component.For<NHConfiguration>()
                .ImplementedBy<ConfigurationMultiTenancy>().LifeStyle.Is(LifestyleType.PerWebRequest));

            container.Register(Component.For<IUnitOfWork>()
                .ImplementedBy<NHUnitOfWorkMultitenancy>()
                .Forward<NHUnitOfWorkMultitenancy>()
                .DependsOn(Dependency.OnComponent<NHConfiguration, ConfigurationMultiTenancy>())
                .LifeStyle.Is(LifestyleType.PerWebRequest));

            container.Register(Component.For(typeof(IRepository<>))
                .ImplementedBy(typeof(NHRepository<>))
                .DependsOn(Dependency.OnComponent<IUnitOfWork, NHUnitOfWorkMultitenancy>())
                .LifeStyle.Is(LifestyleType.PerWebRequest));



            /*
            container.Register(Component.For<IEmailDispatcher>()
                .ImplementedBy<SendGridEmailDispatcher>().LifeStyle.Is(LifestyleType.Singleton));
                */

            container.Register(Component.For<IEmailDispatcher>()
                .ImplementedBy<MemDebugEmailDispatcher>().LifeStyle.Is(LifestyleType.Singleton));

            container.Register(Component.For<MessageFormater<EmailTemplate, EmailMessage>>()
                .ImplementedBy<EmailMessageFormater>()
                    .LifeStyle.Is(LifestyleType.Singleton));

            container.Register(Component.For<MessageTemplateFactory<EmailTemplate>>()
                .ImplementedBy<MessageTemplateFactory<EmailTemplate>>()
                    .LifeStyle.Is(LifestyleType.Singleton));

            container.Register(Component.For<TokenGenerator>()
                .ImplementedBy<TokenGenerator>()
                    .LifeStyle.Is(LifestyleType.Singleton));

            container.Register(Component.For<MessageGenerator<EmailTemplate, EmailMessage>>()
                .ImplementedBy<MessageGenerator<EmailTemplate, EmailMessage>>()
                    .LifeStyle.Is(LifestyleType.Singleton));
        }
    }
}