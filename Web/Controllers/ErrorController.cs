using System.Web.Mvc;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

    }
}
