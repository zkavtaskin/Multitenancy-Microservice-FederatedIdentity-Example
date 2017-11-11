using Server.Core.Domain;
using Server.Core.Time;
using Server.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace Server.Domain.Users
{
    public class UserInvite : IAggregateRoot
    {
        public virtual Guid Id { get; protected set; }

        public virtual string Email { get; protected set; }

        public virtual Guid InvitedByUserId { get; protected set; }

        public virtual DateTime DateCreated { get; protected set; }

        public virtual DateTime DateModified { get; protected set; }

        public virtual Guid InviteKey { get; protected set; }

        public virtual ReadOnlyCollection<Guid> GroupIds
        {
            get
            {
                return this.groupIds.ToList().AsReadOnly();
            }
        }

        private IList<Guid> groupIds;
        private Guid tenantId;

        public static UserInvite Create(Guid userInvitedById, string email)
        {
            if (userInvitedById == Guid.Empty)
                throw new ValidationException("User invited by Id can't be empty");

            if (String.IsNullOrEmpty(email))
                throw new ValidationException("Email can't be empty");

            if (!Regex.IsMatch(email, StringValidation.Email, RegexOptions.IgnoreCase))
                throw new ValidationException("Invalid email specified");

            UserInvite userInvite = new UserInvite();
            userInvite.Id = Guid.NewGuid();
            userInvite.InviteKey = Guid.NewGuid();
            userInvite.Email = email;
            userInvite.InvitedByUserId = userInvitedById;
            userInvite.DateCreated = TimeProvider.Current.UtcNow;
            userInvite.DateModified = TimeProvider.Current.UtcNow;
            userInvite.groupIds = new List<Guid>();

            DomainEvents.Raise<UserInvited>(new UserInvited(userInvite));

            return userInvite;
        }

        public virtual void Add(Domain.Groups.Group group)
        {
            if (group == null)
                throw new ValidationException("Group can't be empty");

            if (this.groupIds.Contains(group.Id))
                throw new ValidationException("User is already invited to this group");

            this.groupIds.Add(group.Id);
        }

        public virtual void Remove(Groups.Group group)
        {
            if (group == null)
                throw new ValidationException("Group can't be empty");

            if (!this.groupIds.Contains(group.Id))
                throw new ValidationException("User was not invited to this group");

            this.groupIds.Remove(group.Id);
        }

        public virtual void ResendInvite()
        {
            this.InviteKey = Guid.NewGuid();
            this.DateModified = TimeProvider.Current.UtcNow;
            DomainEvents.Raise<UserInvited>(new UserInvited(this));
        }

        public override bool Equals(object obj)
        {
            UserInvite compareTo = obj as UserInvite;
            if (compareTo == null)
                throw new Exception("Can't compare objects of different types");

            return compareTo.Email == this.Email && compareTo.tenantId == this.tenantId;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + this.Email.GetHashCode();
            hash = hash * 23 + this.tenantId.GetHashCode();
            return hash;
        }
    }
}
