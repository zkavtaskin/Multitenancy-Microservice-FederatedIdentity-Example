using Microsoft.Owin;

namespace Web.Middleware
{
    public interface ITenantNameExtractor
    {
        string GetName(IOwinContext context);
    }
}
