using Server.Service.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Server.Service;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class SettingsController : LoggedInController
    {
        public SettingsController(IUserService userService, TenantContext tenantContext)
            : base(userService)
        {

        }

        [HttpGet]
        public ActionResult Index()
        {
            UserSettingModel model = new UserSettingModel();
            model.FullName = this.user.FullName;
            model.Email = this.user.Email;

            if(this.user.Contacts.Any())
            {
                model.PrimaryNumber = this.user.Contacts.First().Value;
            }

            if(user.Contacts.Count == 2)
            {
                model.SecondaryNumber = this.user.Contacts[1].Value;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UserSettingModel model)
        {
            if(ModelState.IsValid)
            {
                model.Email = model.Email.ToLower();

                
                if(this.user.Email != model.Email)
                {
                    List<UserDto> users = this.userService.GetUsers();
                    List<UserInviteDto> userInvites = this.userService.GetUserInvites();
                    if(users.Count(x => x.Email == model.Email) != 0 
                        || userInvites.Count(x => x.Email == model.Email) != 0)
                    {
                        model.EmailIsAlreadyUsed = true;
                        return View(model);
                    }
                
                    this.userService.ChangeEmail(this.user.Id, model.Email);
                } 

                this.userService.ChangeName(this.user.Id, model.FullName);

                this.userService.ChangeContact(
                        this.user.Id,
                         new ContactDto()
                         {
                             Type = global::Server.Service.Users.Contact.PrimaryNumber,
                             Value = model.PrimaryNumber
                         }
                     );

                if (!String.IsNullOrEmpty(model.SecondaryNumber))
                {
                    this.userService.ChangeContact(
                                this.user.Id,
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
