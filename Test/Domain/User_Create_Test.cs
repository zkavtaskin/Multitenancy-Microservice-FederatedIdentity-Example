using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.Domain.Users;
using System;
using System.Collections.Generic;
using FluentAssertions;
using Server.Domain.Exceptions;
using Server.Core.Time;

namespace Test.Domain
{
    [TestClass]
    public class User_Create_Test
    {
        Guid tenantId;

        [TestInitialize]
        public void Init()
        {
            this.tenantId = Guid.NewGuid();

            TimeProvider.Current = new TestTimeProvider(new DateTime(2015, 01, 25));
        }

        [TestMethod]
        public void AllProvided_Success()
        {
            Mock<User> expected = new Mock<User>();
            expected.SetupGet(x => x.FullName).Returns("Bob Smith");
            expected.SetupGet(x => x.IDPID).Returns("idpID");
            expected.SetupGet(x => x.Email).Returns("bob.smith@test.com");
            expected.SetupGet(x => x.Initials).Returns("BS");
            expected.SetupGet(x => x.Contacts).Returns((new List<UserContact>()).AsReadOnly());
            expected.SetupGet(x => x.IsAdmin).Returns(true);
            expected.SetupGet(x => x.DateCreated).Returns(new DateTime(2015, 01, 25));
            expected.SetupGet(x => x.DateModified).Returns(new DateTime(2015, 01, 25));

            User actual = User.Create("idpID", "bob.smith@test.com", "Bob Smith", true);

            actual.ShouldBeEquivalentTo
                (
                    expected.Object,
                    options => options.Excluding(p => p.PropertyPath == "Id")
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void idpIDMissing_ExceptionThrown()
        {
            User actual = User.Create(String.Empty, "bob.smith@test.com", "Bob Smith", true);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void EmailMissing_ExceptionThrown()
        {
            User actual = User.Create("idpID", String.Empty, "Bob Smith", true);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void FullNameMissing_ExceptionThrown()
        {
            User actual = User.Create("idpID", "bob.smith@test.com", String.Empty, true);
        }
    }
}
