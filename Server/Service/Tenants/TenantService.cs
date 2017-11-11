using AutoMapper;
using Server.Core.Repository;
using Server.Domain.Tenants;
using System;

namespace Server.Service.Tenants
{
    public class TenantService : ITenantService
    {
        TenantDomainService tenantDomainService;
        IRepository<Tenant> tenantRepo;
        IUnitOfWork unitOfWork;

        public TenantService(IRepository<Tenant> tenantRepo, TenantDomainService tenantDomainService,  IUnitOfWork unitOfWork)
        {
            this.tenantDomainService = tenantDomainService;
            this.tenantRepo = tenantRepo;
            this.unitOfWork = unitOfWork;
        }

        public TenantDto Create(string tenantName, string friendlyName, string timeZoneId, string clientId, Uri authority, string userEmail)
        { 
            this.unitOfWork.BeginTransaction();

            Tenant tenant = Tenant.Create(tenantName, friendlyName, timeZoneId, clientId, authority, userEmail);
            this.tenantRepo.Add(tenant);

            TenantDto tenantDto = Mapper.Map<TenantDto>(tenant);

            this.unitOfWork.Commit();

            return tenantDto;
        }

        public bool IsFriendlyNameAvailable(string name)
        {
            this.unitOfWork.BeginTransaction();

            bool isAvailable = this.tenantDomainService.IsFriendlyNameAvailable(name);

            this.unitOfWork.Commit();

            return isAvailable;
        }

        public string GenerateFriendlyName(string tenantName)
        { 
            return this.tenantDomainService.GenerateFriendlyName(tenantName);
        }


        public TenantDto Get(string friendlyName)
        {
            this.unitOfWork.BeginTransaction();

            friendlyName = friendlyName.ToLower();

            TenantDto tenantDto = Mapper.Map<TenantDto>(this.tenantRepo.FindOne(x => x.NameFriendly == friendlyName));

            this.unitOfWork.Commit();

            return tenantDto;
        }

        public void ChangeTimeZone(Guid tenantId, string timeZoneId)
        {
            this.unitOfWork.BeginTransaction();

            this.tenantDomainService.ChangeTimeZone(tenantId, timeZoneId);

            this.unitOfWork.Commit();
        }
    }
}
