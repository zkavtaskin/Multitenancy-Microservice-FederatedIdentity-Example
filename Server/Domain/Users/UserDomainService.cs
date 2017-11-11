using Server.Core.Repository;
using Server.Domain.Exceptions;
using Server.Domain.Groups;
using Server.Domain.Stops;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Domain.Users
{
    public class UserDomainService
    {
        IRepository<User> userRepo;
        IRepository<UserInvite> userInviteRepo;
        IRepository<Group> groupRepo;
        IRepository<Stop> stopRepo;

        public UserDomainService(IRepository<User> userRepository, 
            IRepository<UserInvite> userInviteRepository, IRepository<Group> groupRepo, IRepository<Stop> stopRepo)
        {
            this.userRepo = userRepository;
            this.userInviteRepo = userInviteRepository;
            this.groupRepo = groupRepo;
            this.stopRepo = stopRepo;
        }

        public void Add(User user)
        {
            if (this.userRepo.Count() == 0)
            {
                this.userRepo.Add(user);
                return;
            }

            if (this.userRepo.Find(x => x.IDPID == user.IDPID).Any())
                throw new ValidationException("User with this IDPID already exists");

            if (this.userRepo.Find(x => x.Email == user.Email).Any())
                throw new ValidationException("User with this email already exists");

            UserInvite userInvite =
                this.userInviteRepo.FindOne(x => x.Email == user.Email);

            if (userInvite == null)
                throw new ValidationException("There is no invite for the user with this email address");

            this.userRepo.Add(user);

            IEnumerable<Group> groups = this.groupRepo.Find(x => userInvite.GroupIds.Contains(x.Id));

            foreach (Group group in groups)
                group.Add(user);

            this.userInviteRepo.Remove(userInvite);
        }

        public void Remove(Guid userId)
        {
            User userToRemove = this.userRepo.FindById(userId);

            if (userToRemove == null)
                throw new  ValidationException("Can't remove user that doesn't exist");

            if (this.stopRepo.Find(x => x.ById == userId && x.WhenResolved == null).Any())
                throw new ValidationException("Can't delete this user, this user has stopped the line and problem is not resolved");

            IEnumerable<Group> groups = this.groupRepo.Find(g => g.Users.Contains(userToRemove));

            foreach (Group group in groups)
            {
                group.Remove(userToRemove);
            }

            this.userRepo.Remove(userToRemove);
        }

        public void ChangeEmailAddress(Guid userId, string email)
        {
            email = email.ToLower();

            User user = this.userRepo.FindById(userId);

            if (user.Email != email)
            {
                if (this.userRepo.Find(x =>  x.Email == email).Any())
                    throw new ValidationException("This email is already in use");

                if (this.userInviteRepo.Find(x => x.Email == email).Any())
                    throw new ValidationException("This email is already in use");

                user.ChangeEmail(email);
            }
        }

        public void ChangeAdminRole(Guid userId, bool isAdmin)
        {
            User userToChange = this.userRepo.FindById(userId);

            if(!isAdmin && this.userRepo.Count(x => x.IsAdmin) < 2)
            {
                throw new ValidationException("There must always be at least one admin");
            }

            userToChange.ChangeAdminRole(isAdmin);
        }

        public void ReInviteUser(Guid userInviteId)
        {
            UserInvite userInvite = this.userInviteRepo.FindById(userInviteId);
            userInvite.ResendInvite();
        }

        public UserInvite InviteUser(Guid userInvitedById, Guid groupId, string email)
        {
            email = email.ToLower();

            Group group = this.groupRepo.FindById(groupId);

            if (this.userRepo.Find(x => x.Email == email).Any())
                throw new ValidationException("This user already exists in the system");

            UserInvite userInvite = this.userInviteRepo.FindOne(x =>  x.Email == email);

            if (userInvite == null)
            {
                userInvite = UserInvite.Create(userInvitedById, email);
                userInvite.Add(group);
                this.userInviteRepo.Add(userInvite);
            }
            else
            {
                userInvite.Add(group);
            }

            return userInvite;
        }

        public void UninviteUser(Guid userInvitedById, Guid groupId, string email)
        {
            email = email.ToLower();

            Group group = this.groupRepo.FindById(groupId);
            UserInvite userInvite = this.userInviteRepo.FindOne(x => x.Email == email);

            if (userInvite == null)
                throw new ValidationException("Can't uninvite unexisting user");

            userInvite.Remove(group);
            if (userInvite.GroupIds.Count == 0)
            {
                this.userInviteRepo.Remove(userInvite);
            }
        }

        public void RemoveUserInvite(Guid userInviteId)
        {
            UserInvite userInvite = this.userInviteRepo.FindById(userInviteId);
            this.userInviteRepo.Remove(userInvite);
        }

    }
}
