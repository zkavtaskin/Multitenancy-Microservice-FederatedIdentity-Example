using Microsoft.Owin;
using System.Threading.Tasks;
using System.Security.Claims;
using Server.Core.Container;
using Server.Service.Users;

namespace Web.Middleware
{
    public class AuthenticationClaimsAppenderMiddleware : OwinMiddleware
    {
        public AuthenticationClaimsAppenderMiddleware(OwinMiddleware next)
            : base(next)
        {

        }

        public override Task Invoke(IOwinContext context)
        {
            IUserService userService = ServiceLocator.Resolve<IUserService>();
            ClaimsPrincipal claimsPrincipal = context.Authentication.User;

            string idpID = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            UserDto userDto = userService.GetUserByIdpID(idpID);
            ClaimsIdentity claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            claimsIdentity.AddClaim(new Claim(ClaimTypesExtension.AppUserId, userDto.Id.ToString()));

            if (userDto.IsAdmin)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            }

            return this.Next.Invoke(context);
        }
    }
}