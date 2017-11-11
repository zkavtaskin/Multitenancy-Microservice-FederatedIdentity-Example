
using Server.Core.Domain;

namespace Server.Domain.Users
{
    public class UserInvited : IDomainEvent
    {
        public UserInvite Invite { get; protected set; }

        public UserInvited(UserInvite invite)
        {
            this.Invite = invite;
        }
    }
}
