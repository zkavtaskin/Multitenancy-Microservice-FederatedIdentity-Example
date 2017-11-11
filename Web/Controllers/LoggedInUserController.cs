using Server.Service.Users;
using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Server.Service;
using System.Security.Claims;
using System.Web.Routing;

namespace Web.Controllers
{
    [Authorize]
    public abstract class LoggedInUserController : Controller
    {
        protected IUserService userService;
        protected UserDto user;
        protected TenantContext tenantContext;

        public LoggedInUserController(IUserService userService, TenantContext tenantContext)
        {
            this.userService = userService;
            this.tenantContext = tenantContext;
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            if (!requestContext.HttpContext.Request.IsAuthenticated)
            {
                requestContext.HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties
                    { 
                        RedirectUri = $"/{tenantContext.FriendlyName}/dashboard/"
                    },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);

                return base.BeginExecute(requestContext, callback, state);
            }

            ClaimsPrincipal claimsPrincipal = ((ClaimsPrincipal)requestContext.HttpContext.User);
            string audience = claimsPrincipal.FindFirst("aud").Value;
            if(audience != this.tenantContext.AuthClientId)
            {
                throw new Exception("Client ID and Audience ID are not matching");
            }

            ClaimsIdentity claimsIdentity = claimsPrincipal.Identity as ClaimsIdentity;
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

            return base.BeginExecute(requestContext, callback, state);
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            if (!this.Request.IsAuthenticated)
            {
                base.EndExecute(asyncResult);
                return;
            }

            string id = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;
            this.user = this.userService.GetUserByIdpID(id);

            this.ViewBag.UserId = this.user.Id;
            this.ViewBag.UserFullName = this.user.FullName;
            this.ViewBag.UserInitials = this.user.Initials;
            this.ViewBag.UserIsManager = this.user.IsAdmin;
            this.ViewBag.TenantFriendlyName = this.tenantContext.FriendlyName;

            base.EndExecute(asyncResult);
        }
    }
}
