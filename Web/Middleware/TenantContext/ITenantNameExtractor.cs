using Microsoft.Owin;

namespace Web.Middleware
{
    public interface ITenantNameExtractor
    {
        bool CanExtract(IOwinContext context);
        string GetName(IOwinContext context);
    }
}
