using Microsoft.Owin;

namespace Web.Middleware
{
    public interface ITenantIdentifierExtractor
    {
        bool CanExtract(IOwinContext context);
        string GetIdentifier(IOwinContext context);
    }
}
