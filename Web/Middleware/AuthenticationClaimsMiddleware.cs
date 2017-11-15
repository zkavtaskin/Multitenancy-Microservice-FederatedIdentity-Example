using Microsoft.Owin;
using System.Threading.Tasks;
using System.Security.Claims;
using Server.Core.Container;
using Server.Service.Users;

namespace Web.Middleware
{
    public class AuthenticationClaimsMiddleware : OwinMiddleware
    {
        public AuthenticationClaimsMiddleware(OwinMiddleware next)
            : base(next)
        {

        }

        public override Task Invoke(IOwinContext context)
        {
            IUserService userService = ServiceLocator.Resolve<IUserService>();
            ClaimsPrincipal claimsPrincipal = context.Authentication.User;

            string idpID = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
            UserDto userDto = userService.GetUserByIdpID(idpID);

            if (userDto.IsAdmin)
            {
                ClaimsIdentity claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            }

            return this.Next.Invoke(context);
        }
    }
}