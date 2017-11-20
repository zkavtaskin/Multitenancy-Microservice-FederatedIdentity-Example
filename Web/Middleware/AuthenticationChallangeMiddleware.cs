using Microsoft.Owin;
using System.Threading.Tasks;
using Server.Service;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;

namespace Web.Middleware
{
    public class AuthenticationChallangeMiddleware : OwinMiddleware
    {
        readonly TenantContext tenantContext;

        public AuthenticationChallangeMiddleware(OwinMiddleware next, TenantContext tenantContext)
            : base(next)
        {
            this.tenantContext = tenantContext;
        }

        public override Task Invoke(IOwinContext context)
        {
            ClaimsPrincipal claimsPrincipal = context.Authentication.User;
            if (!claimsPrincipal.Identity.IsAuthenticated)
            {
                context.Authentication.Challenge(new AuthenticationProperties
                {
                    RedirectUri = context.Request.Uri.AbsoluteUri
                }, OpenIdConnectAuthenticationDefaults.AuthenticationType);

                return Task.FromResult<int>(0);
            }

            return this.Next.Invoke(context);
        }
    }
}