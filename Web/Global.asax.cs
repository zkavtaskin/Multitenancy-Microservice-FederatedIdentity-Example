using System;
using System.Web;
using log4net;
using Server.Core.Container;

namespace Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception serverException = this.Server.GetLastError();

            HttpException httpException = serverException as HttpException;
            if (httpException?.GetHttpCode() == 404)
                return;

            ILog logger = ServiceLocator.Resolve<ILog>();
            Guid uniqueId = Guid.NewGuid();
            logger.Fatal(uniqueId, serverException);
        }
    }
}