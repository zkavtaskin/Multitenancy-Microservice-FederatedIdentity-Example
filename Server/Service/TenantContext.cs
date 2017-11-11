using System;

namespace Server.Service
{
    public sealed class TenantContext
    {
        public  Guid ID { get; private set; }
        public string FriendlyName { get; private set; }
        public string AuthClientId { get; private set; }
        public string AuthAuthority { get; private set; }

        public TenantContext()
        {
            
        }

        public TenantContext(Guid id, string friendlyName, string authClientId, string authAuthority)
        {
            this.ID = id;
            this.FriendlyName = friendlyName;
            this.AuthClientId = authClientId;
            this.AuthAuthority = authAuthority;
        }

        public bool IsEmpty()
        {
            return this.ID == Guid.Empty;
        }

        public override bool Equals(object obj)
        {
            TenantContext compareTo = obj as TenantContext;
            if (compareTo == null)
                throw new Exception("Can't compare objects of different types");

            return compareTo.ID == this.ID;
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }
    }
}