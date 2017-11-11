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
    public class User : IAggregateRoot
    {
        public virtual Guid Id { get; protected set; }
        public virtual string FullName { get; protected set; }
        public virtual string Initials
        {
            get
            {
                if (String.IsNullOrEmpty(this.FullName))
                    return null;

                string initials = string.Empty;
                this.FullName.Split(' ').ToList().ForEach(new Action<string>((split) =>
                {
                    initials += split.Substring(0, 1).ToUpper();
                }));

                return initials;
            }
        }
        public virtual string Email { get; protected set; }
        public virtual bool IsAdmin { get; protected set; }
        public virtual ReadOnlyCollection<UserContact> Contacts { get { return contacts.ToList().AsReadOnly(); } }
        public virtual DateTime DateCreated { get; protected set; }
        public virtual DateTime DateModified { get; protected set; }

        public virtual string IDPID { get; protected set; }

        private IList<UserContact> contacts;
        private Guid tenantId;

        public static User Create(string idpID, string  email, string fullName, bool isAdmin)
        {
            if (string.IsNullOrEmpty(idpID))
                throw new ValidationException("idpID is null or empty");

            if (string.IsNullOrEmpty(email))
                throw new ValidationException("email is null or empty");

            if (string.IsNullOrEmpty(fullName))
                throw new ValidationException("fullname is null or empty");

            User user = new User();
            user.Id = Guid.NewGuid();
            user.IDPID = idpID;
            user.contacts = new List<UserContact>();
            user.ChangeFullName(fullName);
            user.ChangeEmail(email);
            user.ChangeAdminRole(isAdmin);
            user.DateCreated = TimeProvider.Current.UtcNow;
            user.DateModified = TimeProvider.Current.UtcNow;
            return user;
        }

        public virtual void Add(UserContact userContact)
        {
            if (userContact == null)
                throw new ValidationException("User contact can't be empty");

            if (this.contacts.Contains(userContact))
                throw new ValidationException("User contact already exists");

            this.contacts.Add(userContact);

            this.DateModified = TimeProvider.Current.UtcNow;
        }

        public virtual void Remove(UserContact userContact)
        {
            if (userContact == null)
                throw new ValidationException("User contact can't Be Empty");

            if (!this.contacts.Contains(userContact))
                throw new ValidationException("User contact doesn't Exist");

            this.contacts.Remove(userContact);

            this.DateModified = TimeProvider.Current.UtcNow;
        }

        public virtual void ChangeFullName(string fullName)
        {
            if (String.IsNullOrEmpty(fullName))
                throw new ValidationException("Full Name can't be empty");

            this.FullName = fullName;

            this.DateModified = TimeProvider.Current.UtcNow;
        }

        public virtual void ChangeEmail(string email)
        {
            if (String.IsNullOrEmpty(email))
                throw new ValidationException("Email can't be empty");

            if (!Regex.IsMatch(email, StringValidation.Email, RegexOptions.IgnoreCase))
                throw new ValidationException("Invalid email specified");

            this.Email = email.ToLower();

            this.DateModified = TimeProvider.Current.UtcNow;
        }

        public virtual void ChangeAdminRole(bool isAdmin)
        {
            this.IsAdmin = isAdmin;

            this.DateModified = TimeProvider.Current.UtcNow;
        }

        public override bool Equals(object obj)
        {
            User compareTo = obj as User;
            if (compareTo == null)
                throw new Exception("Can't compare objects of different types");

            return compareTo.IDPID == this.IDPID && compareTo.Email == this.Email && compareTo.tenantId == this.tenantId;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + this.IDPID.GetHashCode();
            hash = hash * 23 + this.Email.GetHashCode();
            hash = hash * 23 + this.tenantId.GetHashCode();
            return hash;
        }
    }
}
