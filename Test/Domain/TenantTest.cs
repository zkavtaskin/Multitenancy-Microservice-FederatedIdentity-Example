using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.Domain.Tenants;
using System;
using FluentAssertions;
using Server.Domain.Exceptions;
using Server.Core.Time;

namespace Test.Domain
{
    [TestClass]
    public class TenantTest
    {
        Tenant tenant;

        [TestInitialize]
        public void Init()
        {
            TimeProvider.Current = new TestTimeProvider(new DateTime(2015, 01, 25));
            this.tenant = Tenant.Create("tenant", "tenant", "GMT Standard Time", "clientid", new Uri("http://somewebsite.com"), "bob.smith@test.com");
        }

        [TestMethod]
        public void Create_AllInformationIsProvided_Success()
        {
            Mock<Tenant> expected = new Mock<Tenant>();
            expected.SetupGet(x => x.Name).Returns("Test");
            expected.SetupGet(x => x.NameFriendly).Returns("Test");
            expected.SetupGet(x => x.TimeZoneId).Returns("GMT Standard Time");
            expected.SetupGet(x => x.ClientId).Returns("clientid");
            expected.SetupGet(x => x.Authority).Returns(new Uri("http://somewebsite.com"));
            expected.SetupGet(x => x.DateCreated).Returns(new DateTime(2015, 01, 25));
            expected.SetupGet(x => x.DateModified).Returns(new DateTime(2015, 01, 25));

            Tenant actual = Tenant.Create("Test", "Test", "GMT Standard Time", "clientid", new Uri("http://somewebsite.com"), "bob.smith@test.com");

            actual.ShouldBeEquivalentTo
                (
                    expected.Object,
                    options => options.Excluding(p => p.PropertyPath == "Id")
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_TenantNameIsMissing_ThrowsException()
        {
            Tenant tenant = Tenant.Create(String.Empty, String.Empty, String.Empty, String.Empty, null, String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_TenantFriendlyNameIsMissing_ThrowsException()
        {
            Tenant tenant = Tenant.Create("Test", String.Empty, String.Empty, String.Empty, null, String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_TenantTimeZoneMissing_ThrowsException()
        {
            Tenant.Create("Test", "Test", String.Empty, String.Empty, null, String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_ClientIDMissing_ThrowsException()
        {
            Tenant.Create("Test", "Test", "TimeZoneId", String.Empty, null, String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_AuthorityMissing_ThrowsException()
        {
            Tenant.Create("Test", "Test", "TimeZoneId", "ClientID", null, String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_InitUserEmailMissing_ThrowsException()
        {
            Tenant.Create("Test", "Test", "TimeZoneId", "ClientID", new Uri("http://somewebsite.com"), String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ChangeTimeZone_EmptyTimeZonePassed_ThrowsException()
        {
            this.tenant.ChangeTimeZone(String.Empty);
        }

        [TestMethod]
        public void ChangeTimeZone_TimeZonePassed_TimeZoneChanged()
        {
            string expected = "USA";

            this.tenant.ChangeTimeZone("USA");

            this.tenant.TimeZoneId.Should().Be(expected);
        }


        [TestMethod]
        public void Verify_Verified()
        {
            bool expected = true;

            this.tenant.Verify();

            this.tenant.Verified.Should().Be(expected);
        }

    }
}
