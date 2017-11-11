using AutoMapper;
using Server.Core.Repository;
using Server.Domain.Groups;
using Server.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Service.Groups
{
    public class GroupService : IGroupService
    {
        GroupDomainService groupDomainService;
        UserDomainService userDomainService;
        IRepository<Group> groupRepo;
        IRepository<User> userRepo;
        IRepository<UserInvite> userInviteRepo;
        IUnitOfWork unitOfWork;

        public GroupService(GroupDomainService groupDomainService,
            IRepository<User> userRepo, IRepository<Group> groupRepository, UserDomainService userDomainService,
            IRepository<UserInvite> userInviteRepo, IUnitOfWork unitOfWork)
        {
            this.groupDomainService = groupDomainService;
            this.groupRepo = groupRepository;
            this.userRepo = userRepo;
            this.userDomainService = userDomainService;
            this.userInviteRepo = userInviteRepo;
            this.unitOfWork = unitOfWork;
        }

        public GroupDto Create(string name)
        {            
            this.unitOfWork.BeginTransaction();

            Group group = Group.Create(name);
            this.groupDomainService.Add(group);

            GroupDto groupDto = Mapper.Map<GroupDto>(group);

            this.unitOfWork.Commit();

            return groupDto;

        }
       
        public GroupDto AddUser(Guid groupId, Guid userId)
        {
            this.unitOfWork.BeginTransaction();

            GroupDto groupDto = Mapper.Map<GroupDto>(this.groupDomainService.AddUser(groupId, userId));

            this.unitOfWork.Commit();

            return groupDto;

        }

        public GroupDto RemoveUser(Guid groupId, Guid userId)
        {
            this.unitOfWork.BeginTransaction();

            GroupDto groupDto = Mapper.Map<GroupDto>(this.groupDomainService.RemoveUser(groupId, userId));

            this.unitOfWork.Commit();

            return groupDto;
        }

        public List<GroupDto> Remove(Guid groupId)
        {
            this.unitOfWork.BeginTransaction();

            this.groupDomainService.Remove(groupId);

            List<GroupDto> groupDtos = Mapper.Map<List<GroupDto>>(this.groupDomainService.GetGroups());

            this.unitOfWork.Commit();

            return groupDtos;
        }

        public List<GroupDto> GetGroups()
        {
            this.unitOfWork.BeginTransaction();

            List<GroupDto> groupDtos = Mapper.Map<List<GroupDto>>(this.groupDomainService.GetGroups().OrderBy(g => g.Name));

            this.unitOfWork.Commit();

            return groupDtos;
        }

        public List<GroupUserDto> GetUsers(Guid groupId)
        {
            this.unitOfWork.BeginTransaction();

            List<GroupUserDto> groupUserDtos = Mapper.Map<List<GroupUserDto>>(this.groupRepo.FindById(groupId).Users.OrderBy(x => x.FullName));

            this.unitOfWork.Commit();

            return groupUserDtos;
        }


        public List<GroupInvitedUserDto> InviteUser(Guid userInvitedById, Guid groupId, string email)
        {
            this.unitOfWork.BeginTransaction();

            this.userDomainService.InviteUser(userInvitedById, groupId, email);

            List<GroupInvitedUserDto> groupInvitedUserDtos =
                Mapper.Map<List<GroupInvitedUserDto>>(this.userInviteRepo.Find(ui => ui.GroupIds.Contains(groupId)));

            this.unitOfWork.Commit();

            return groupInvitedUserDtos;
        }

        public List<GroupInvitedUserDto> UninviteUser(Guid userUninvitedById, Guid groupId, string email)
        {
            this.unitOfWork.BeginTransaction();

            this.userDomainService.UninviteUser(userUninvitedById, groupId, email);

            List<GroupInvitedUserDto> groupInvitedUserDtos =
                Mapper.Map<List<GroupInvitedUserDto>>(this.userInviteRepo.Find(ui => ui.GroupIds.Contains(groupId)));

            this.unitOfWork.Commit();

            return groupInvitedUserDtos;
        }


        public List<GroupInvitedUserDto> GetInvitedUsers(Guid groupId)
        {
            this.unitOfWork.BeginTransaction();

            Group group = this.groupRepo.FindById(groupId);
            List<GroupInvitedUserDto> groupInvitedUserDtos =
                Mapper.Map<List<GroupInvitedUserDto>>(group.Invites.OrderBy(x => x.Email));

            this.unitOfWork.Commit();

            return groupInvitedUserDtos;

        }

        public GroupDto GetGroup(Guid groupId)
        {
            this.unitOfWork.BeginTransaction();

            GroupDto groupDto = Mapper.Map<GroupDto>(this.groupRepo.FindById(groupId));

            this.unitOfWork.Commit();

            return groupDto;
        }

        public bool IsNameAvailable(string groupName)
        {
            this.unitOfWork.BeginTransaction();

            bool isNameAvailable = !this.groupRepo.Find(group => group.Name == groupName.Trim()).Any();

            this.unitOfWork.Commit();

            return isNameAvailable;
        }
    }
}
