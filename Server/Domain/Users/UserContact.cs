using Server.Domain.Exceptions;
using System;

namespace Server.Domain.Users
{
    public class UserContact
    {
        public virtual Guid Id { get; protected set; }

        public virtual string Value { get; protected set; }
        public virtual Contact Type { get; protected set; }
        public virtual Guid UserId { get; protected set; }

        public static UserContact Create(User user, Contact type, string value)
        {
            if (user == null)
                throw new ValidationException("User can't be empty");

            if (String.IsNullOrEmpty(value))
                throw new ValidationException("Value can't be empty");

            UserContact userContact = new UserContact();
            userContact.Id = Guid.NewGuid();
            userContact.Type = type;
            userContact.UserId = user.Id;
            userContact.Value = value;
            return userContact;
        }

        public override bool Equals(object obj)
        {
            UserContact compareTo = obj as UserContact;
            if (compareTo == null)
                throw new Exception("Can't compare objects of different types");

            return compareTo.Type == this.Type;
        }

        public override int GetHashCode()
        {
            return this.Type.GetHashCode();
        }
    }
}
