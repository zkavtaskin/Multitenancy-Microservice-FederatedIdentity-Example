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

        public LoggedInController(IUserService userService)
        {
            this.userService = userService;
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            string id = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;
            this.user = this.userService.GetUserByIdpID(id);

            this.ViewBag.UserId = user.Id;
            this.ViewBag.UserFullName = user.FullName;
            this.ViewBag.UserInitials = user.Initials;
            this.ViewBag.UserIsManager = user.IsAdmin;

            base.EndExecute(asyncResult);
        }
    }
}
