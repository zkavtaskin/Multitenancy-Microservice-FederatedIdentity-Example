using Server.Core.Repository;
using Server.Domain.Exceptions;
using Server.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Domain.Groups
{
    public class GroupDomainService
    {
        IRepository<Group> groupRepo;
        IRepository<User> userRepo;

        public GroupDomainService(IRepository<Group> groupRepo, IRepository<User> userRepo)
        {
            this.groupRepo = groupRepo;
            this.userRepo = userRepo;
        }

        public void Add(Group group)
        {
            if(this.groupRepo.Find(x => x.Name.Contains(group.Name)).Any())
                throw new ValidationException("Group already exists");

            this.groupRepo.Add(group);
        }

        public void Remove(Guid groupId)
        {
            Group group = this.groupRepo.FindById(groupId);

            foreach (UserInvite userInvite in group.Invites)
            {
                group.Remove(userInvite);
            }

            foreach (User user in group.Users)
            {
                group.Remove(user);
            }

            this.groupRepo.Remove(group);
        }

        public List<Group> GetGroups()
        {
            return this.groupRepo.GetAll().ToList();
        }

        public Group AddUser(Guid groupId, Guid userId)
        {
            Group group = this.groupRepo.FindById(groupId);
            User user = this.userRepo.FindById(userId);
            group.Add(user);
            return group;
        }

        public Group RemoveUser(Guid groupId, Guid userId)
        {
            Group group = this.groupRepo.FindById(groupId);
            User user = this.userRepo.FindById(userId);
            group.Remove(user);
            return group;
        }
    }
}
