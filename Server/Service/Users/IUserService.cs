using System;
using System.Collections.Generic;

namespace Server.Service.Users
{
    public interface IUserService
    {
        List<UserDto> GetUsers();
        UserDto Add(string idpID, string email, string fullName, List<ContactDto> contacts, bool isAdmin);
        void Remove(Guid userId);

        void RemoveInvite(Guid userInviteId);

        void ResendInvite(Guid userInviteId);

        UserInviteDto GetUserInvite(Guid inviteKey);

        void ChangeName(Guid userId, string fullName);

        void ChangeContact(Guid userId, ContactDto contact);

        void ChangeEmail(Guid userId, string email);

        void ChangeAdminRole(Guid userId, bool isAdmin);

        UserDto GetUser(string email);

        UserDto GetUserByIdpID(string IDPID);

        List<UserInviteDto> GetUserInvites();
    }
}
