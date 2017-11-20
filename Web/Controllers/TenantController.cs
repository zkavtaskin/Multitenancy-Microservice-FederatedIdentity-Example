using Server.Service.Tenants;
using System;
using System.Linq;
using System.Web.Mvc;
using Web.Models;
using System.Web;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class TenantController : Controller
    {
        ITenantService tenantService;

        public TenantController(ITenantService tenantService)
        {
            this.tenantService = tenantService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new TenantCreateModel() { 
                TimeZones = TimeZoneInfo.GetSystemTimeZones().ToList(),
                TimeZoneId = "GMT Standard Time"
            });
        }

        [HttpPost]
        public ActionResult Index(TenantCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                model.TimeZones = TimeZoneInfo.GetSystemTimeZones().ToList();
                return View(model);
            }

            TenantDto tenantDto = this.tenantService.Create(model.TenantName, model.TenantFriendlyName, 
                model.TimeZoneId, model.ClientID, new Uri(model.Authority), model.UserEmail);

            return Redirect($"/{tenantDto.NameFriendly}/usersetup/init/?initKey={tenantDto.Id}&email={ HttpUtility.HtmlEncode(model.UserEmail)}");
        }

        [HttpGet]
        public JsonResult GenerateFriendlyName(string name)
        {
            TenantNameModel model = new TenantNameModel();
            model.Name = name;
            model.FriendlyName = this.tenantService.GenerateFriendlyName(model.Name);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult TenantFriendlyNameCheck(string friendlyName)
        {
            FriendlyNameCheckModel model = new FriendlyNameCheckModel();
            model.Name = friendlyName;
            model.IsAvailable = this.tenantService.IsFriendlyNameAvailable(model.Name);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}
