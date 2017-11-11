using System;
using System.Collections.Generic;

namespace Server.Service.Stops
{
    public interface IStopService
    {
        StopDto Create(Guid userId, Guid groupId, string problem);
        StopDto ProblemResolved(Guid userId, Guid stopId);
        List<StopDto> GetUnresolved();
        List<StopDto> GetAll(Guid groupId);
    }
}
