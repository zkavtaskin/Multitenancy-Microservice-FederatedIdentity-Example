using AutoMapper;
using Server.Core.Repository;
using Server.Domain.Stops;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Service.Stops
{
    public class StopService : IStopService
    {
        IRepository<Stop> stopRepo;
        StopDomainService stopDomainService;
        IUnitOfWork unitOfWork;

        public StopService(IRepository<Stop> stopRepository, IUnitOfWork unitOfWork, 
            StopDomainService stopDomainService)
        {
            this.stopDomainService = stopDomainService;
            this.stopRepo = stopRepository;
            this.unitOfWork = unitOfWork;
        }

        public StopDto Create(Guid userId, Guid groupId, string problem)
        {
            this.unitOfWork.BeginTransaction();

            StopDto stopDto = Mapper.Map<StopDto>(stopDomainService.Create(groupId, problem, userId));

            this.unitOfWork.Commit();

            return stopDto;
        }

        public StopDto ProblemResolved(Guid userId, Guid stopId)
        {
            this.unitOfWork.BeginTransaction();

            Stop stop = this.stopRepo.FindById(stopId);
            stop.ProblemResolved(userId);

            StopDto stopDto = Mapper.Map<StopDto>(stop);

            this.unitOfWork.Commit();

            return stopDto;
        }

        public List<StopDto> GetUnresolved()
        {
            this.unitOfWork.BeginTransaction();

            List<StopDto> stopDtos = Mapper.Map<List<StopDto>>(this.stopRepo.Find(stop => stop.WhenResolved == null));

            this.unitOfWork.Commit();

            return stopDtos;
        }

        public List<StopDto> GetAll(Guid groupId)
        {
            this.unitOfWork.BeginTransaction();

            List<StopDto> stopDtos = Mapper.Map<List<StopDto>>(this.stopRepo.Find(stop => stop.GroupId == groupId)
                .OrderByDescending(stop => stop.Date));

            this.unitOfWork.Commit();

            return stopDtos;
        }
    }
}
