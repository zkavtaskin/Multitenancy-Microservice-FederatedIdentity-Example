using Castle.Windsor;
using log4net;
using Server.Core.Web;
using System.Web.Mvc;
using Web.App_Start.Install;

namespace Web.App_Start
{
    public class BootstrapConfig
    {
        public static void Register(IWindsorContainer container, ILog logger)
        {
            WindsorControllerFactory controllerFactory =
                new WindsorControllerFactory(container.Kernel);

            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            container.Install(
                    new InfrastructureInstall(),
                    new DomainInstall(),
                    new ServiceInstall(logger),
                    new ControllerInstall()
                );
        }
    }
}