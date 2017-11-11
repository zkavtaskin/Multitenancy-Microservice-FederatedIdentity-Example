using Server.Core.Domain;
using Server.Core.Time;
using Server.Domain.Exceptions;
using Server.Domain.Groups;
using Server.Domain.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Server.Domain.Stops
{
    public class Stop : IAggregateRoot
    {
        public virtual Guid Id { get; protected set; }
        public virtual string Problem { get; protected set; }

        public virtual string By { get; protected set; }

        public virtual DateTime Date { get; protected set; }

        public virtual Nullable<DateTime> WhenResolved { get; protected set; }

        public virtual string GroupName { get; protected set; }

        public virtual Guid GroupId { get; protected set; }

        public virtual Guid ById { get; protected set; }

        public virtual ReadOnlyCollection<String> GroupUsers
        {
            get
            {
                return this.groupUsers.ToList().AsReadOnly();
            }
        }

        public virtual Nullable<TimeSpan> OverallDownTime
        {
            get
            {
                if (!this.WhenResolved.HasValue)
                    return null;

                return this.WhenResolved - this.Date;
            }
        }

        private Guid tenantId;
        private IList<string> groupUsers;

        public static Stop Create(Group group, User by, string problem)
        {
            if (group == null)
                throw new ValidationException("Group can't be empty");

            if (by == null)
                throw new ValidationException("User can't be empty");

            if (String.IsNullOrEmpty(problem))
                throw new ValidationException("Problem can't be empty");

            if (!group.Users.Contains(by))
                throw new ValidationException("Only user in the group");

            Stop stop = new Stop();
            stop.Id = Guid.NewGuid();
            stop.Problem = problem;
            stop.Date = TimeProvider.Current.UtcNow;
            stop.groupUsers = new List<string>();
            stop.By = by.FullName;
            stop.ById = by.Id;
            stop.GroupName = group.Name;
            stop.GroupId = group.Id;

            foreach (User groupUser in group.Users)
                stop.groupUsers.Add(groupUser.FullName);

            DomainEvents.Raise<Stopped>(new Stopped(stop));

            return stop;
        }
        public virtual void ProblemResolved(Guid userResolvedBy)
        {
            if (userResolvedBy != this.ById)
                throw new ValidationException("Only the same user can resolve the problem that was raised");

            this.WhenResolved = TimeProvider.Current.UtcNow;

            DomainEvents.Raise<Resumed>(new Resumed(this));
        }
    }
}
