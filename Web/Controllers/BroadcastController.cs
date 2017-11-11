using AutoMapper;
using Server.Service.Groups;
using Server.Service.Stops;
using Server.Service.Users;
using System.Collections.Generic;
using System.Web.Mvc;
using Server.Service;
using Web.Models;
using Web.Models.Broadcast;

namespace Web.Controllers
{
    public class BroadcastController : LoggedInUserController
    {
        IGroupService groupService;
        IStopService stopService;

        public BroadcastController(IGroupService groupService, IUserService userService, 
            IStopService stopService, TenantContext tenantContext)
            : base(userService, tenantContext)
        {
            this.groupService = groupService;
            this.stopService = stopService;
        }

        public ActionResult Index()
        {
            return View(this.getBroadcastModel());
        }

        [HttpGet]
        public ActionResult Broadcast()
        {
            return PartialView(this.getBroadcastModel());
        }

        private Model getBroadcastModel()
        {
            Model model = new Model();
            List<StopDto> unresolvedGroups = this.stopService.GetUnresolved();
            model.Groups = Mapper.Map<List<StopDto>, List<GroupModel>>(unresolvedGroups);

            return model;
        }

    }
}
