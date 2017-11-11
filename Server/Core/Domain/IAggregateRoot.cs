using System;

namespace Server.Core.Domain
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}