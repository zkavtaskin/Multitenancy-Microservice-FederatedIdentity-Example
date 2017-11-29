using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Service.Tenants
{
    public class TTenantRecord
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NameFriendly { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string TimeZoneId { get; set; }
        public string AuthClientId { get; set; }
        public string AuthAuthority { get; set; }
    }
}
