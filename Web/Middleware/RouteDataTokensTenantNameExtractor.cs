using System;
using System.Web;
using System.Web.Routing;
using Microsoft.Owin;

namespace Web.Middleware
{
    public class RouteDataTokensTenantNameExtractor : ITenantNameExtractor
    {
        readonly RouteData routeData;

        public RouteDataTokensTenantNameExtractor(IOwinContext context)
        {
            HttpContextBase httpContext = (HttpContextBase)context.Environment["System.Web.HttpContextBase"];
            this.routeData = RouteTable.Routes.GetRouteData(httpContext);
        }

        public bool CanExtract()
        {
            return routeData != null && routeData.DataTokens.ContainsValue("webclient_multitenancy");
        }

        public string GetName()
        {
            if (this.CanExtract())
                return routeData.GetRequiredString("tenant");

            return null;
        }
    }
}