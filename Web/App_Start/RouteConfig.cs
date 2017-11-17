using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("bundles/{*pathInfo}");
            routes.IgnoreRoute("signalr/{*pathInfo}");

            routes.MapRoute(
                "defaultTenantSignUp",
                "signup/tenant/{action}/{id}",
                new {controller = "tenant", action = "Index", id = UrlParameter.Optional}
            );

            routes.MapRoute(
                "error",
                "error",
                new {controller = "error", action = "error", id = UrlParameter.Optional}
            );

            routes.MapRoute(
                "multi",
                "{tenant}/{controller}/{action}/{id}",
                new {controller = "Dashboard", action = "Index", id = UrlParameter.Optional}
            ).DataTokens.Add("name", "webclient_multitenancy");

        }
    }
}