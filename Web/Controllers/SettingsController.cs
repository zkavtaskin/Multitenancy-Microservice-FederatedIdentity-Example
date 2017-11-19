using Server.Service.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Server.Service;
using Web.Middleware;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        IUserService userService;

        public SettingsController(IUserService userService, TenantContext tenantContext)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            UserDto user = this.userService.GetUserByIdpID(((ClaimsPrincipal) this.User).IdentityProviderUserId());

            UserSettingModel model = new UserSettingModel();
            model.FullName = user.FullName;
            model.Email = user.Email;

            if(user.Contacts.Any())
            {
                model.PrimaryNumber = user.Contacts.First().Value;
            }

            if(user.Contacts.Count == 2)
            {
                model.SecondaryNumber = user.Contacts[1].Value;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UserSettingModel model)
        {
            if(ModelState.IsValid)
            {
                model.Email = model.Email.ToLower();

                UserDto user = this.userService.GetUserByIdpID(((ClaimsPrincipal)this.User).IdentityProviderUserId());

                if (user.Email != model.Email)
                {
                    List<UserDto> users = this.userService.GetUsers();
                    List<UserInviteDto> userInvites = this.userService.GetUserInvites();
                    if(users.Count(x => x.Email == model.Email) != 0 
                        || userInvites.Count(x => x.Email == model.Email) != 0)
                    {
                        model.EmailIsAlreadyUsed = true;
                        return View(model);
                    }
                
                    this.userService.ChangeEmail(user.Id, model.Email);
                } 

                this.userService.ChangeName(user.Id, model.FullName);

                this.userService.ChangeContact(
                    user.Id,
                    new ContactDto()
                    {
                        Type = global::Server.Service.Users.Contact.PrimaryNumber,
                        Value = model.PrimaryNumber
                    }
                );

                if (!String.IsNullOrEmpty(model.SecondaryNumber))
                {
                    this.userService.ChangeContact(
                        user.Id,
                        new ContactDto()
                        {
                            Type = global::Server.Service.Users.Contact.SecondaryNumber,
                            Value = model.SecondaryNumber
                        }
                    );
                }

            }
            return View(model);
        }

    }
}
