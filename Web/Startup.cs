using Microsoft.Owin;
using Owin;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Optimization;
using Server.Core.Container;
using AutoMapper;
using Castle.Windsor;
using log4net;
using Server.Service;
using Web.App_Start;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Web.Middleware;
using Microsoft.Owin.Security.Cookies;
using System.Web.Helpers;
using System.IdentityModel.Claims;
using Server.Service.Tenants;
using Web.Extensions;

[assembly: OwinStartup(typeof(Web.Startup))]
namespace Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog logger = LogManager.GetLogger("Logger");

            logger.Info("Application Started");
            logger.Info("Configuring DI container");
            IWindsorContainer container = new WindsorContainer();
            ServiceLocator.Set(container); 
            BootstrapConfig.Register(container, logger);

            logger.Info("Configuring HTTP Middleware");
            app.MapSignalR();
            app.UseMultitenancy(new MultitenancyNotifications<TenantDto>
            {
                TenantNameNotFound = context =>
                {
                    throw new HttpException(400, "Tenant name must be provided");
                },
                TenantRecordNotFound = context =>
                {
                    context.Response.Redirect("/signup/tenant/");
                    return Task.FromResult(0);
                },
                CreateTenantContext = (context, tenantRecord) =>
                {
                    ITenantContextFactory tenantContextFactory = ServiceLocator.Resolve<ITenantContextFactory>();
                    TenantContext tenantContext = tenantContextFactory.Create(tenantRecord.Id, tenantRecord.NameFriendly, tenantRecord.AuthClientId, tenantRecord.AuthAuthority);
                    return Task.FromResult(tenantContext);
                }
            });
            app.UsePerTenant((tenantContext, appBranch) =>
            {
                appBranch.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

                appBranch.UseCookieAuthentication(new CookieAuthenticationOptions
                { 
                    CookieName = $"OAuthCookie.{tenantContext.FriendlyName}"
                });

                appBranch.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
                {
                    ClientId = tenantContext.AuthClientId,
                    Authority = tenantContext.AuthAuthority,
                    RedirectUri = $"http://localhost:2295/{tenantContext.FriendlyName}/",
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        AuthenticationFailed = context =>
                        {
                            context.HandleResponse();
                            throw context.Exception;
                        }
                    }
                });

                appBranch.Use<AuthenticationChallangeMiddleware>();
                appBranch.Use<AuthenticationAudienceCheckMiddleware>(tenantContext);
                appBranch.Use<AuthenticationClaimsAppenderMiddleware>();
            });

            logger.Info("Configuring MVC Pipeline");
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            logger.Info("Configuring Domain, DTO and Model Mapping");
            MappingConfig.RegisterMapping();
            Mapper.AddProfile(new Web.Models.Map());

            app.OnDispose(() =>
            {
                ServiceLocator.Resolve<ILog>().Info("Application Ended");
                ServiceLocator.Release();
            });
        }

    }

}
