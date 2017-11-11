using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Moq;
using Server.Core.Repository;
using Server.Domain.Tenants;
using System.Collections.Generic;
using Server.Domain.Users;
using System.Linq.Expressions;

namespace Test.Domain
{
    [TestClass]
    public class TenantDomainServiceTest
    {
        Mock<IRepository<Tenant>> tenantRepo;
        TenantDomainService tenantsDomainService;


        [TestInitialize]
        public void Init()
        {
            this.tenantRepo = new Mock<IRepository<Tenant>>();

            this.tenantsDomainService =
                new TenantDomainService(tenantRepo.Object);
        }

        [TestMethod]
        public void IsFriendlyNameAvailable_IsAvailable_True()
        {
            this.tenantRepo.Setup(x => x.Find(It.IsAny<Expression<Func<Tenant, bool>>>())).Returns(
                   new List<Tenant>()
                   {
                   }
                );

            this.tenantsDomainService.IsFriendlyNameAvailable("test").Should().BeTrue();
        }

        [TestMethod]
        public void IsFriendlyNameAvailable_IsNotAvailable_False()
        {
            Mock<Tenant> tenant = new Mock<Tenant>();
            tenant.SetupGet(x => x.NameFriendly).Returns("test");

            this.tenantRepo.Setup(x => x.Count(It.IsAny<Expression<Func<Tenant, bool>>>())).Returns(1);

            tenantsDomainService.IsFriendlyNameAvailable("test").Should().BeFalse();
        }

        [TestMethod]
        public void GenerateFriendlyName_MatchesActual()
        {
            List<Tuple<string, string>> expectedNames = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Test", "test"),
                new Tuple<string, string>("Some Company", "some_company"), 
                new Tuple<string, string>(" Some Company ", "some_company"), 
                new Tuple<string, string>(" S0me C0mpany ", "s0me_c0mpany")
            };

            foreach (Tuple<string, string> expectedName in expectedNames)
            {
                tenantsDomainService.GenerateFriendlyName(expectedName.Item1).Should().Be(expectedName.Item2);
            }
        }
    }
}
