using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
            ).DataTokens.Add("name", "apiclient_multitenancy");

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
