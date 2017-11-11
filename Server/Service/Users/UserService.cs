using AutoMapper;
using Server.Core.Repository;
using Server.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Service.Users
{
    public class UserService : IUserService
    {
        IRepository<User> userRepository;
        IRepository<UserInvite> userInviteRepository;
        UserDomainService userDomainService;
        IUnitOfWork unitOfWork;

        public UserService(IRepository<User> userRepository, UserDomainService userDomainService,
            IRepository<UserInvite> userInviteRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.userDomainService = userDomainService;
            this.userInviteRepository = userInviteRepository;
            this.unitOfWork = unitOfWork;
        }

        public List<UserDto> GetUsers()
        {
            this.unitOfWork.BeginTransaction();

            List<UserDto> userDtos = Mapper.Map<List<UserDto>>(this.userRepository.GetAll());

            this.unitOfWork.Commit();

            return userDtos;
        }

        public UserDto Add(string idpID, string email, string fullName, List<ContactDto> contacts, bool isAdmin)
        {
            this.unitOfWork.BeginTransaction();

            User user = User.Create(idpID, email, fullName, isAdmin);

            foreach (ContactDto contact in contacts)
            {
                user.Add(UserContact.Create(user, (Domain.Users.Contact)contact.Type, contact.Value));
            }

            this.userDomainService.Add(user);

            UserDto userDto = Mapper.Map<UserDto>(user);

            this.unitOfWork.Commit();

            return userDto;
        }

        public void Remove(Guid userId)
        {
            this.unitOfWork.BeginTransaction();

            this.userDomainService.Remove(userId);

            this.unitOfWork.Commit();
        }

        public void RemoveInvite(Guid userInviteId)
        {
            this.unitOfWork.BeginTransaction();

            this.userDomainService.RemoveUserInvite(userInviteId);

            this.unitOfWork.Commit();
        }

        public void ResendInvite(Guid userInviteId)
        {
            this.unitOfWork.BeginTransaction();

            this.userDomainService.ReInviteUser(userInviteId);

            this.unitOfWork.Commit();
        }

        public UserInviteDto GetUserInvite(Guid inviteKey)
        {
            this.unitOfWork.BeginTransaction();

            UserInviteDto userInviteDto = Mapper.Map<UserInviteDto>(this.userInviteRepository.FindOne(userInvite => userInvite.InviteKey == inviteKey));

            this.unitOfWork.Commit();

            return userInviteDto;
        }

        public void ChangeName(Guid userId, string fullName)
        {
            this.unitOfWork.BeginTransaction();

            User user = this.userRepository.FindById(userId);
            user.ChangeFullName(fullName);

            this.unitOfWork.Commit();
        }

        public void ChangeContact(Guid userId, ContactDto contact)
        {
            this.unitOfWork.BeginTransaction();

            User user = this.userRepository.FindById(userId);

            UserContact userContact =
                user.Contacts.FirstOrDefault(x => x.Type == (Domain.Users.Contact)contact.Type);

            UserContact userContactNew =
                UserContact.Create(user, (Domain.Users.Contact)contact.Type, contact.Value);

            if (userContact != null)
            {
                user.Remove(userContact);
                user.Add(userContactNew);
            }
            else
            {
                user.Add(userContactNew);
            }

            this.unitOfWork.Commit();
        }

        public void ChangeEmail(Guid userId, string email)
        {
            this.unitOfWork.BeginTransaction();

            this.userDomainService.ChangeEmailAddress(userId, email);

            this.unitOfWork.Commit();
        }

        public void ChangeAdminRole(Guid userId, bool isAdmin)
        {
            this.unitOfWork.BeginTransaction();

            this.userDomainService.ChangeAdminRole(userId, isAdmin);

            this.unitOfWork.Commit();
        }

        public UserDto GetUser(string email)
        {
            this.unitOfWork.BeginTransaction();

            email = email.ToLower();

            UserDto userDto = Mapper.Map<UserDto>(this.userRepository.FindOne(u => u.Email == email));

            this.unitOfWork.Commit();

            return userDto;
        }

        public UserDto GetUserByIdpID(string IDPID)
        {
            this.unitOfWork.BeginTransaction();

            UserDto userDto = Mapper.Map<UserDto>(this.userRepository.FindOne(u => u.IDPID == IDPID));

            this.unitOfWork.Commit();

            return userDto;
        }

        public List<UserInviteDto> GetUserInvites()
        {
            this.unitOfWork.BeginTransaction();

            List<UserInviteDto> userInviteDtos = Mapper.Map<List<UserInviteDto>>(this.userInviteRepository.GetAll().OrderBy(u => u.Email));

            this.unitOfWork.Commit();

            return userInviteDtos;
        }
    }
}
