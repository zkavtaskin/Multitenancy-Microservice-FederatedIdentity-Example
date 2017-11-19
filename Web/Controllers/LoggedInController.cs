using Server.Service.Users;
using System;
using System.Web.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    [Authorize]
    public abstract class LoggedInController : Controller
    {
        protected readonly IUserService userService;
        protected UserDto user;

        protected LoggedInController(IUserService userService)
        {
            this.userService = userService;
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            string id = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;
            this.user = this.userService.GetUserByIdpID(id);

            this.ViewBag.UserFullName = user.FullName;
            this.ViewBag.UserInitials = user.Initials;

            base.EndExecute(asyncResult);
        }
    }
}
