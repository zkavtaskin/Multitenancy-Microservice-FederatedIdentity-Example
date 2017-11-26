using System;
using Microsoft.Owin;
using System.Threading.Tasks;
using Server.Service;
using System.Security.Claims;

namespace Web.Middleware
{
    public class AuthenticationAudienceCheckMiddleware : OwinMiddleware
    {
        readonly TenantContext tenantContext;

        public AuthenticationAudienceCheckMiddleware(OwinMiddleware next, TenantContext tenantContext)
            : base(next)
        {
            this.tenantContext = tenantContext;
        }

        public override Task Invoke(IOwinContext context)
        {
            ClaimsPrincipal claimsPrincipal = context.Authentication.User;
            string audience = claimsPrincipal.FindFirst(ClaimTypesExtension.Audience).Value;
            if (audience != this.tenantContext.AuthClientId)
            {
                throw new Exception("Client ID and Audience ID are not matching");
            }

            return this.Next.Invoke(context);
        }
    }
}