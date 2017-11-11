using System.Web.Mvc;
using Server.Service.Users;
using System.Web;
using Microsoft.Owin.Security.Cookies;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        IUserService userService;

        public LoginController(IUserService userService)
        {
            this.userService = userService;
        }
        
        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return View();
        }
    }
}