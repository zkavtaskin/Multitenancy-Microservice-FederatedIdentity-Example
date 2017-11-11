using log4net;
using System.Web.Mvc;

namespace Web.Filters
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        ILog log;

        public CustomHandleErrorAttribute(ILog log)
        {
            this.log = log;
        }

        public override void OnException(ExceptionContext filterContext)
        {
            log.Error($"Exception, View:{this.View}, Controller: {filterContext.Controller}", filterContext.Exception);
            base.OnException(filterContext);
        }
    }
}