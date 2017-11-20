using Server.Service.Users;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Web.Models;
using Server.Service;
using System.Security.Claims;
using System.Linq;

namespace Web.Controllers
{
    [Authorize]
    public class UserSetupController : Controller
    {
        IUserService userService;
        TenantContext tenantContext;

        public UserSetupController(IUserService userService, TenantContext tenantContext)
        {
            this.userService = userService;
            this.tenantContext = tenantContext;
        }

        [HttpGet]
        public ActionResult Index(Guid inviteKey)
        {
            UserInviteDto userInvite = this.userService.GetUserInvite(inviteKey);

            if(userInvite == null)
                return View("InvalidKey");

            return View(new UserSetupModel() { Email = userInvite.Email, Key = inviteKey });
        }

        [HttpGet]
        public ActionResult Init(Guid initKey, string email)
        {
            if (initKey != this.tenantContext.ID)
                return View("InvalidKey");

            if(this.userService.GetUsers().Any())
                return View("InvalidKey");

            return View("InitIndex", new UserSetupModel() { Email = email, Key = initKey });
        }

        [HttpPost]
        public ActionResult Create(UserSetupModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            UserInviteDto userInvite = this.userService.GetUserInvite(model.Key);
            if (userInvite == null)
            {
                return View("InvalidKey");
            }

            this.createUser(model, userInvite.Email, false);

            return RedirectToAction("Index", "Dashboard");
        }


        [HttpPost]
        public ActionResult CreateInitUser(UserSetupModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Key != this.tenantContext.ID)
                return View("InvalidKey");

            if (this.userService.GetUsers().Any())
                return View("InvalidKey");

            this.createUser(model, model.Email, true);

            return RedirectToAction("Index", "Dashboard");
        }

        private void createUser(UserSetupModel model, string email, bool isAdmin)
        {
            var idpID = ((ClaimsPrincipal)this.User).FindFirst(ClaimTypes.NameIdentifier).Value;

            List<ContactDto> contacts = new List<ContactDto>();
            contacts.Add(new ContactDto() { Value = model.PrimaryNumber, Type = global::Server.Service.Users.Contact.PrimaryNumber });

            if (!string.IsNullOrEmpty(model.SecondaryNumber))
                contacts.Add(new ContactDto() { Value = model.SecondaryNumber, Type = global::Server.Service.Users.Contact.SecondaryNumber });

            this.userService.Add(idpID, model.Email, model.FullName, contacts, isAdmin);
        }

    }
}
