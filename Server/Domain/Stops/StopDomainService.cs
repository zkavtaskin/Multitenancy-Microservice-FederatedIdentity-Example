using Server.Core.Repository;
using Server.Domain.Exceptions;
using Server.Domain.Groups;
using Server.Domain.Users;
using System;
using System.Linq;

namespace Server.Domain.Stops
{
    public class StopDomainService
    {
        IRepository<Stop> stopRepo;
        IRepository<Group> groupRepo;
        IRepository<User> userRepo;

        public StopDomainService(IRepository<Stop> stopRepo, IRepository<Group> groupRepo, 
            IRepository<User> userRepo)
        {
            this.stopRepo = stopRepo;
            this.groupRepo = groupRepo;
            this.userRepo = userRepo;
        }

        public Stop Create(Guid groupId, string problem, Guid userId)
        {
            Group group = this.groupRepo.FindById(groupId);

            if (group == null)
                throw new ValidationException("Group is not found");

            User user = this.userRepo.FindById(userId);

            if (!group.Users.Contains(user))
            {
                throw new ValidationException("User doesn't belong to the group so he can't stop the line");
            }

            if (this.stopRepo
                .Find(s => s.GroupId == groupId && s.WhenResolved == null).Any())
            {
                throw new ValidationException("Group is already stopped");
            }

            Stop stop = Stop.Create(group, user, problem);

            this.stopRepo.Add(stop);

            return stop;
        }
    }
}
