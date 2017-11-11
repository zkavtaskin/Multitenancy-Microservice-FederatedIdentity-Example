using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Moq;
using Server.Domain.Users;
using Server.Domain.Groups;
using Server.Domain.Stops;
using System.Collections.Generic;
using Server.Core.Time;
using Server.Domain.Exceptions;

namespace Test.Domain
{
    [TestClass]
    public class StopTest_Create
    {
        Guid tenantId = Guid.NewGuid();
        Mock<User> user;
        Mock<Group> group;

        [TestInitialize]
        public void Init()
        {
            TimeProvider.Current = new TestTimeProvider(new DateTime(2015, 01, 25, 9, 0, 0));

            this.tenantId = Guid.NewGuid();

            this.user = new Mock<User>();
            this.group = new Mock<Group>();
        }

        [TestMethod]
        public void Create_AllInformationProvided_Succesfull()
        {
            this.group.SetupGet(x => x.Name).Returns("Phoenix");

            User user = User.Create("idpID", "bob@smith.com", "Bob Smith", false);

            Mock<User> userBob = new Mock<User>();
            userBob.SetupGet(x => x.FullName).Returns("Bob Smith");

            Mock<User> userJason = new Mock<User>();
            userJason.SetupGet(x => x.FullName).Returns("Jason Smith");

            this.group.SetupGet(x => x.Users).Returns(
                new List<User>()
                {
                    user,
                    userJason.Object
                }.AsReadOnly());

            Mock<Stop> expected = new Mock<Stop>();
            expected.SetupGet(x => x.Problem).Returns("build server");
            expected.SetupGet(x => x.By).Returns(user.FullName);
            expected.SetupGet(x => x.ById).Returns(user.Id);
            expected.SetupGet(x => x.GroupName).Returns("Phoenix");
            expected.SetupGet(x => x.GroupId).Returns(this.group.Object.Id);
            expected.SetupGet(x => x.Date).Returns(new DateTime(2015, 01, 25, 9, 0, 0));
            expected.SetupGet(x => x.GroupUsers).Returns(new List<string>()
                {
                    user.FullName,
                    "Jason Smith"
                }.AsReadOnly());
            expected.SetupGet(x => x.OverallDownTime).Returns(null as TimeSpan?);

            Stop actual = Stop.Create(group.Object, user, "build server");

            actual.ShouldBeEquivalentTo(expected.Object,
                options => options.Excluding(p => p.PropertyPath == "Id"));
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_UserIsNotPartOfTheGroup_ThrowsException()
        {
            this.group.SetupGet(x => x.Users).Returns(new List<User>().AsReadOnly());

            Stop.Create(this.group.Object, 
                this.user.Object, "why");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_GroupIsNull_ThrowsException()
        {
            Stop.Create(null, this.user.Object, "why");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_UserIsNull_ThrowsException()
        {
            Stop.Create(this.group.Object, null, "why");
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_WhyIsEmpty_ThrowsException()
        {
            Stop.Create(this.group.Object, this.user.Object, string.Empty);
        }

    }
}
