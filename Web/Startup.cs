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
using Microsoft.Owin.BuilderProperties;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Web.Middleware;
using Microsoft.Owin.Security.Cookies;
using System.Web.Helpers;
using System.IdentityModel.Claims;
using Server.Service.Users;


[assembly: OwinStartup(typeof(Web.Startup))]

namespace Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog logger = LogManager.GetLogger("Logger");

            IWindsorContainer container = new WindsorContainer();
            ServiceLocator.Set(container); 
            BootstrapConfig.Register(container, logger);

            app.MapSignalR()
               .UseMultitenancy(
                    new WhenMiddleware((context) => {
                        HttpContextBase httpContext = (HttpContextBase)context.Environment["System.Web.HttpContextBase"];
                        RouteData routeData = RouteTable.Routes.GetRouteData(httpContext);
                        return Task.FromResult(routeData != null && routeData.DataTokens.ContainsValue("multi"));
                    }),
                    new MultitenancyNotifications {
                        TenantNameCouldNotBeFound = context =>
                        {
                            throw new HttpException(400, "Tenant name must be provided");
                        },
                        TenantCouldNotBeResolved = context =>
                        {
                            context.Response.Redirect("/signup/tenant/");
                            return Task.FromResult(0);
                        },
                        TenantResolved = (context, tenantContextFactory, tenantDto) =>
                        {
                            tenantContextFactory.Create(
                                tenantDto.Id,
                                tenantDto.NameFriendly,
                                tenantDto.AuthClientId,
                                tenantDto.AuthAuthority
                            );
                            return Task.FromResult(0);
                        }
                    }
                )
                .UsePerTenant((tenantContext, appBranch) => {
                    appBranch.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

                    appBranch.UseCookieAuthentication(new CookieAuthenticationOptions
                        {
                            CookieName = $"OAuthCookie.{tenantContext.FriendlyName}"
                        })
                        .UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions()
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
                        })
                        .Use<AuthenticationChallangeMiddleware>(tenantContext)
                        .Use<AuthenticationAudienceCheckMiddleware>(tenantContext)
                        .Use<AuthenticationClaimsMiddleware>();
                });

            MappingConfig.RegisterMapping();
            Mapper.AddProfile(new Web.Models.Map());

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            logger.Info("Application Started");

            AppProperties properties = new AppProperties(app.Properties);
            CancellationToken token = properties.OnAppDisposing;
            if (token != CancellationToken.None)
            {
                token.Register(() =>
                {
                    ServiceLocator.Resolve<ILog>().Info("Application Ended");
                    ServiceLocator.Release();
                });
            }
        }

    }

}
