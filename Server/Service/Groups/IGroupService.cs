
using System;
using System.Collections.Generic;

namespace Server.Service.Groups
{
    public interface IGroupService
    {
        GroupDto Create(string name);
        GroupDto AddUser(Guid groupId, Guid userId);

        List<GroupInvitedUserDto> InviteUser(Guid userInvitedById, Guid groupId, string email);

        List<GroupInvitedUserDto> UninviteUser(Guid userUninvitedById, Guid groupId, string email);

        List<GroupInvitedUserDto> GetInvitedUsers(Guid groupId);

        GroupDto RemoveUser(Guid groupId, Guid userId);
        List<GroupDto> GetGroups();
        List<GroupUserDto> GetUsers(Guid groupId);

        GroupDto GetGroup(Guid groupId);

        bool IsNameAvailable(string groupName);

        List<GroupDto> Remove(Guid groupId);
    }
}
