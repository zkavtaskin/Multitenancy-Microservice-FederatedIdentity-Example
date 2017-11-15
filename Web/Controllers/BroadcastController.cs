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
    public class BroadcastController : LoggedInController
    {
        readonly IStopService stopService;
        readonly TenantContext tenantContext;

        public BroadcastController(IGroupService groupService, IUserService userService, 
            IStopService stopService, TenantContext tenantContext)
            : base(userService)
        {
            this.stopService = stopService;
            this.tenantContext = tenantContext;
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
            Model model = new Model(this.tenantContext.FriendlyName);
            List<StopDto> unresolvedGroups = this.stopService.GetUnresolved();
            model.Groups = Mapper.Map<List<StopDto>, List<GroupModel>>(unresolvedGroups);

            return model;
        }

    }
}
