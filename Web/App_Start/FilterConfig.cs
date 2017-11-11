using System.Web.Mvc;
using Web.Filters;

namespace Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(Server.Core.Container.ServiceLocator.Resolve<CustomHandleErrorAttribute>());
        }
    }
}