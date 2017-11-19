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

        protected LoggedInController(IUserService userService)
        {
            this.userService = userService;
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            string id = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;
            UserDto userDto = this.userService.GetUserByIdpID(id);

            this.ViewBag.UserFullName = userDto.FullName;
            this.ViewBag.UserInitials = userDto.Initials;

            base.EndExecute(asyncResult);
        }
    }
}
