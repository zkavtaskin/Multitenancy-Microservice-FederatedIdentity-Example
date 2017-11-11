using System.Web.Mvc;

namespace Web.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }

    }
}
