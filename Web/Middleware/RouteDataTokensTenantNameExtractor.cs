using System.Web;
using System.Web.Routing;
using Microsoft.Owin;

namespace Web.Middleware
{
    public class RouteDataTokensTenantNameExtractor : ITenantNameExtractor
    {
        public string GetName(IOwinContext context)
        {

            string tenantName = context.Request.Headers["tenantname"];

            HttpContextBase httpContext = (HttpContextBase) context.Environment["System.Web.HttpContextBase"];
            RouteData routeData = RouteTable.Routes.GetRouteData(httpContext);
            if (routeData != null && routeData.DataTokens.ContainsValue("multi"))
                return routeData.GetRequiredString("tenant");

            return null;
        }
    }
}