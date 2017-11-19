
using AutoMapper;
using Server.Service.Groups;
using Server.Service.Stops;
using Server.Service.Tenants;
using Server.Service.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Server.Service;
using Web.Hubs;
using Web.Middleware;
using Web.Models.Group;

namespace Web.Controllers
{
    [Authorize]
    public class GroupsController : LoggedInController
    {
        IGroupService groupService;
        IStopService stopService;
        ITenantService tenantService;
        TenantContext tenantContext;

        public GroupsController(IGroupService groupService, IUserService userService, 
            IStopService stopService, ITenantService tenantService, TenantContext tenantContext)
            : base(userService)
        {
            this.groupService = groupService;
            this.stopService = stopService;
            this.tenantService = tenantService;
            this.tenantContext = tenantContext;
        }

        [HttpGet]
        public ActionResult Index()
        {
            GroupsModel model = new GroupsModel();

            List<StopDto> stops = this.stopService.GetUnresolved();
            List<GroupDto> groups = this.groupService.GetGroups();

            Dictionary<Guid, StopDto> stopHash = new Dictionary<Guid, StopDto>();
            if (stops != null)
            {
                stopHash = stops.ToDictionary(x => x.GroupId);
            }

            List<Model> groupModels = new List<Model>();
            foreach(GroupDto group in groups)
            {
                Model groupModel = Mapper.Map<GroupDto, Model>(group);
                if(stopHash.ContainsKey(group.Id))
                {
                    groupModel.State = Web.Models.Group.State.Stopped;
                }
                groupModels.Add(groupModel);
            }

            model.Groups = groupModels;
           
            return View(model);
        }

        [HttpGet]
        public ActionResult View(Guid groupId)
        {
            UsersModel model = new UsersModel();

            GroupDto group = this.groupService.GetGroup(groupId);
            model.GroupId = group.Id;
            model.Name = group.Name;
            model.DateCreated = group.DateCreated;

            List<GroupUserDto> groupUsers = this.groupService.GetUsers(groupId);
            List<GroupInvitedUserDto> invitedUsers = this.groupService.GetInvitedUsers(groupId);
            List<StopDto> stops = this.stopService.GetAll(groupId);
            TenantDto tenant = this.tenantService.Get(this.tenantContext.FriendlyName);

            foreach(StopDto stop in stops)
            {
                stop.Date = TimeZoneInfo.ConvertTimeFromUtc(stop.Date, 
                                TimeZoneInfo.FindSystemTimeZoneById(tenant.TimeZoneId));
            }

            IEnumerable<StopSumModel> stopMonths =
                stops.Where(x => x.OverallDownTime.HasValue)
                .GroupBy(x => new { x.Date.Month, x.Date.Year })
                .Select(x => new StopSumModel()
                {
                    Date = new DateTime(x.Key.Year, x.Key.Month, 1),
                    TotalDownTime = TimeSpan.FromMilliseconds(x.Sum(row => row.OverallDownTime.Value.TotalMilliseconds)),
                    Stops = Mapper.Map<List<StopDto>, List<StopModel>>(x.ToList())
                });

            model.Users.AddRange(Mapper.Map<List<GroupUserDto>, List<UserModel>>(groupUsers));
            model.Users.AddRange(Mapper.Map<List<GroupInvitedUserDto>, List<UserModel>>(invitedUsers));

            model.MonthStops.AddRange(stopMonths);

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult RemoveUser(UserModel model)
        {
            if(model.IsInvited)
            {
                this.groupService.UninviteUser(((ClaimsPrincipal)this.User).AppUserId(), model.GroupId, model.Email);
            }
            else
            {
                this.groupService.RemoveUser(model.GroupId, model.Id);
            }

            return RedirectToAction("View", new { groupid = model.GroupId });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ResendVerification(UserModel model)
        {
            this.userService.ResendInvite(model.Id);

            return RedirectToAction("View", new { groupid = model.GroupId });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult RemoveGroup(Model model)
        {
            this.groupService.Remove(model.Id);

            Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<GroupHub>()
                .Clients.All.groupLifeCycleStateChange(this.tenantContext.FriendlyName, LifeCycleState.Removed.ToString());

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]

        public JsonResult Search(string term)
        {
            term = term.ToLower();

            List<UserSuggestionModel> suggestions = new List<UserSuggestionModel>();

            List<UserDto> users = this.userService.GetUsers();

            UserDto userFound = users.FirstOrDefault(user => user.Email == term);

            if(userFound != null)
            {
                suggestions.Add(Mapper.Map<UserDto, UserSuggestionModel>(userFound));
                return Json(suggestions, JsonRequestBehavior.AllowGet);
            }

            List<UserInviteDto> userInvites = this.userService.GetUserInvites();

            UserInviteDto userInviteFound = userInvites.FirstOrDefault(userInvite => userInvite.Email == term);
            if(userInviteFound != null)
            {
                suggestions.Add(Mapper.Map<UserInviteDto, UserSuggestionModel>(userInviteFound));
                return Json(suggestions, JsonRequestBehavior.AllowGet);
            }

            IEnumerable<UserDto> usersFiltered = users.Where(user => user.Email.Contains(term));
            IEnumerable<UserInviteDto> userInvitesFiltered = userInvites.Where(invite => invite.Email.Contains(term));

            suggestions.AddRange(Mapper.Map<IEnumerable<UserDto>, List<UserSuggestionModel>>(usersFiltered));
            suggestions.AddRange(Mapper.Map<IEnumerable<UserInviteDto>, List<UserSuggestionModel>>(userInvitesFiltered));

            suggestions.Add(new UserSuggestionModel());

            return Json(suggestions, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Create()
        {
            return View(new CreateModel());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(CreateModel model)
        {
            if(ModelState.IsValid)
            {
                bool nameIsAvailable = this.groupService.IsNameAvailable(model.Name);

                if (!nameIsAvailable)
                {
                    model.NameIsAvailable = false;
                    return View(model); 
                }

                GroupDto group = this.groupService.Create(model.Name);

                List<string> emails = new List<string>();

                if (!string.IsNullOrEmpty(model.Invited))
                    emails = model.Invited.Split(';').Where(s => !String.IsNullOrEmpty(s)).ToList();

                foreach (string email in emails)
                {
                    UserDto user = this.userService.GetUser(email);

                    if (user == null)
                    {
                        this.groupService.InviteUser(((ClaimsPrincipal)this.User).AppUserId(), group.Id, email);
                    }
                    else
                    {
                        this.groupService.AddUser(group.Id, user.Id);
                    }
                }

                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<GroupHub>()
                    .Clients.All.groupLifeCycleStateChange(this.tenantContext.FriendlyName, LifeCycleState.Added.ToString());

                return RedirectToAction("View", new { groupId = group.Id });
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddUserToGroup(UserInviteModel model)
        {
            if (ModelState.IsValid)
            {

                UserDto user = this.userService.GetUser(model.Email);

                if (user == null)
                {
                    this.groupService.InviteUser(((ClaimsPrincipal)this.User).AppUserId(), model.GroupId, model.Email);
                }
                else
                {
                    this.groupService.AddUser(model.GroupId, user.Id);
                }
            }

            return RedirectToAction("View", new { groupId = model.GroupId });
        }
    }
}
