using AutoMapper;
using Server.Service.Stops;
using Server.Service.Tenants;
using Server.Service.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Server.Service;
using Web.Models.TenantSettings;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TenantSettingsController : LoggedInUserController
    {
        IStopService stopService;
        ITenantService tenantService;

        public TenantSettingsController(IUserService userService, 
            IStopService stopService, ITenantService tenantService, TenantContext tenantContext)
            : base(userService, tenantContext)
        {
            this.stopService = stopService;
            this.tenantService = tenantService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            UsersModel model = new UsersModel();

            List<StopDto> stops = this.stopService.GetUnresolved();
            Dictionary<Guid, StopDto> unresolvedStopsHash = new Dictionary<Guid, StopDto>();

            if(stops != null)
            {
               unresolvedStopsHash = stops.ToDictionary(x => x.ById);
            } 

            List<UserDto> users = this.userService.GetUsers();
            List<UserInviteDto> usersInvited = this.userService.GetUserInvites();

            model.Users = Mapper.Map<List<UserDto>, List<UserModel>>(users);

            foreach (UserModel user in model.Users)
            {
                user.CanRemove = !unresolvedStopsHash.ContainsKey(user.Id);
            }

            model.UserInvites = Mapper.Map<List<UserInviteDto>, List<UserInviteModel>>(usersInvited);

            TimeModel timeZoneModel = new TimeModel();

            timeZoneModel.TimeZones = TimeZoneInfo.GetSystemTimeZones().ToList();

            TenantDto tenant = this.tenantService.Get(this.tenantContext.FriendlyName);

            timeZoneModel.TimeZoneId = tenant.TimeZoneId;

            model.Time = timeZoneModel;

            return View(model);
        }

        [HttpPost]
        public ActionResult RemoveInvitedUser(UserInviteModel model)
        {
            this.userService.RemoveInvite(model.Id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ResendVerification(UserInviteModel model)
        {
            this.userService.ResendInvite(model.Id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveUser(UserModel model)
        {
            this.userService.Remove(model.Id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddAdmin(UserModel model)
        {
            this.userService.ChangeAdminRole(model.Id, true);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveAdmin(UserModel model)
        {
            this.userService.ChangeAdminRole(model.Id, false);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ChangeTime(TimeModel model)
        {
            this.tenantService.ChangeTimeZone(this.tenantContext.ID, model.TimeZoneId);

            return RedirectToAction("Index");
        }

    }
}
