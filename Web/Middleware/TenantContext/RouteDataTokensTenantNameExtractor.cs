using System;
using System.Web;
using System.Web.Routing;
using Microsoft.Owin;

namespace Web.Middleware
{
    public class RouteDataTokensTenantNameExtractor : ITenantNameExtractor
    {
        public bool CanExtract(IOwinContext context)
        {
            RouteData routeData = this.getRouteData(context);
            return routeData != null && routeData.DataTokens.ContainsValue("webclient_multitenancy");
        }

        public string GetName(IOwinContext context)
        {
            if (!this.CanExtract(context))
                return null;

            RouteData routeData = this.getRouteData(context);
            return routeData.GetRequiredString("tenant");
        }

        private RouteData getRouteData(IOwinContext context)
        {
            HttpContextBase httpContext = (HttpContextBase)context.Environment["System.Web.HttpContextBase"];
            return RouteTable.Routes.GetRouteData(httpContext);
        }
    }
}