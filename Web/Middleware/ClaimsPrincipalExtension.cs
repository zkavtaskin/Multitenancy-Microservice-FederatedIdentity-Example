using System;
using System.Security.Claims;

namespace Web.Middleware
{
    public static class ClaimsPrincipalExtension
    {

        public static Guid AppUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return Guid.Parse(claimsPrincipal.FindFirst(ClaimTypesExtension.AppUserId).Value);
        }

        public static string IdentityProviderUserId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}