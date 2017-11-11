using Server.Core.Domain;
using Server.Core.Time;
using Server.Domain.Exceptions;
using System;

namespace Server.Domain.Tenants
{
    public class Tenant : IAggregateRoot
    {
        public virtual Guid Id
        {
            get;
            protected set;
        }

        public virtual string Name { get; protected set; }

        public virtual string NameFriendly { get; protected set; }

        public virtual string TimeZoneId { get; protected set; }

        public virtual string ClientId { get; protected set; }
        public virtual Uri Authority { get; protected set; }

        public virtual bool Verified { get; protected set; }

        public virtual DateTime DateCreated { get; protected set; }
        public virtual DateTime DateModified { get; protected set; }


        public static Tenant Create(string name, string friendlyName, string timeZoneId, string clientId, Uri authority, string initUserEmail)
        {
            if (String.IsNullOrEmpty(name))
                throw new ValidationException("Tenant name can't be empty");

            if (String.IsNullOrEmpty(friendlyName))
                throw new ValidationException("Tenant friendly name can't be empty");

            if (!System.Text.RegularExpressions.Regex.IsMatch(name, StringValidation.TenantName, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                throw new ValidationException("Tenant name is invalid");

            if (!System.Text.RegularExpressions.Regex.IsMatch(friendlyName, StringValidation.TenantNameFriendly, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                throw new ValidationException("Tenant friendly name is invalid");

            if (String.IsNullOrEmpty(timeZoneId))
                throw new ValidationException("Time Zone Id can't be empty");

            if (String.IsNullOrEmpty(clientId))
                throw new ValidationException("Client Id can't be empty");

            if (authority == null)
                throw new ValidationException("Authority URI can't be empty");

            if (String.IsNullOrEmpty(initUserEmail))
                throw new ValidationException("User email can't be empty");

            if (!System.Text.RegularExpressions.Regex.IsMatch(initUserEmail, StringValidation.Email, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                throw new ValidationException("Email is invalid");

            Tenant tenant = new Tenant()
            {
                Id = Guid.NewGuid(),
                Name = name.Trim(),
                NameFriendly = friendlyName,
                TimeZoneId = timeZoneId,
                ClientId = clientId,
                Authority = authority,
                Verified = false,
                DateCreated = TimeProvider.Current.UtcNow,
                DateModified = TimeProvider.Current.UtcNow
            };

            DomainEvents.Raise<TenantCreated>(new TenantCreated(tenant, initUserEmail));

            return tenant;
        }

        public virtual void ChangeTimeZone(string timeZoneId)
        {
            if (String.IsNullOrEmpty(timeZoneId))
                throw new ValidationException("Time Zone can't be empty");

            this.TimeZoneId = timeZoneId;
            this.DateModified = TimeProvider.Current.UtcNow;
        }

        public virtual void Verify()
        {
            this.Verified = true;
            this.DateModified = TimeProvider.Current.UtcNow;
        }

    }
}
