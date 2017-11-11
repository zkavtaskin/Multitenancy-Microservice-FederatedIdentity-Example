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
                name: "defaultTenantSignUp",
                url: "signup/tenant/{action}/{id}",
                defaults: new { controller = "tenant", action = "Index" , id = UrlParameter.Optional }
            ).DataTokens.Add("name", "defaultTenantSignUp");

            routes.MapRoute(
                name: "error",
                url: "error",
                defaults: new { controller = "error", action = "error", id = UrlParameter.Optional }
            ).DataTokens.Add("name", "error");

            routes.MapRoute(
                name: "multi",
                url: "{tenant}/{controller}/{action}/{id}",
                defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            ).DataTokens.Add("name", "multi");

        }
    }
}