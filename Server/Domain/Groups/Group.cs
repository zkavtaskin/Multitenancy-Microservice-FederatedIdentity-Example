using Server.Core.Domain;
using Server.Core.Time;
using Server.Domain.Exceptions;
using Server.Domain.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Server.Domain.Groups
{
    public class Group : IAggregateRoot
    {
        public virtual Guid Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual ReadOnlyCollection<User> Users
        {
            get
            {
                return this.users.ToList().AsReadOnly();
            }
        }

        public virtual ReadOnlyCollection<UserInvite> Invites
        {
            get
            {
                return this.invites.ToList().AsReadOnly();
            }
        }

        public virtual DateTime DateCreated { get; protected set; }
        public virtual DateTime DateModified { get; protected set; }

        private Guid tenantId;
        private IList<User> users;
        private IList<UserInvite> invites;

        public static Group Create(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ValidationException("Name can't be empty");

            Group group = new Group();
            group.Id = Guid.NewGuid();
            group.Name = name;
            group.users = new List<User>();
            group.invites = new List<UserInvite>();
            group.DateCreated = TimeProvider.Current.UtcNow;
            group.DateModified = TimeProvider.Current.UtcNow;
            return group;
        }

        public virtual void Add(User user)
        {
            if (user == null)
                throw new ValidationException("User can't be empty");

            if (this.users.Contains(user))
                throw new ValidationException("User is already in this group");

            this.users.Add(user);

            this.DateModified = TimeProvider.Current.UtcNow;
        }

        public virtual void Remove(User user)
        {
            if (user == null)
                throw new ValidationException("User can't be empty");

            if (!this.users.Contains(user))
                throw new ValidationException("Use is not in the group");

            this.users.Remove(user);

            this.DateModified = TimeProvider.Current.UtcNow;
        }


        public virtual void Add(UserInvite userInvite)
        {
            if (userInvite == null)
                throw new ValidationException("UserInvite can't be empty");

            if (this.invites.Contains(userInvite))
                throw new ValidationException("UserInvite is already in the group");

            this.invites.Add(userInvite);

            this.DateModified = TimeProvider.Current.UtcNow;
        }

        public virtual void Remove(UserInvite userInvite)
        {
            if (userInvite == null)
                throw new ValidationException("UserInvite can't be empty");

            if (!this.invites.Contains(userInvite))
                throw new ValidationException("UserInvite is not in the group");

            this.invites.Remove(userInvite);

            this.DateModified = TimeProvider.Current.UtcNow;
        }
    }
}
