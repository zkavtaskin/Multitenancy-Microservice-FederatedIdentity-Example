using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Domain.Groups;
using Moq;
using FluentAssertions;
using Server.Domain.Exceptions;
using Server.Domain.Users;
using System.Collections.Generic;
using Server.Core.Time;


namespace Test.Domain
{
    [TestClass]
    public class GroupTest
    {
        Guid tenantId;
        Group group;

        [TestInitialize]
        public void Init()
        {
            TimeProvider.Current = new TestTimeProvider(new DateTime(2015, 01, 25));

            this.tenantId = Guid.NewGuid();
            this.group = Group.Create("Phoenix - V2");
        }

        [TestMethod]
        public void Create_AllInformationIsProvided_Succesfull()
        {
            Mock<Group> expected = new Mock<Group>();
            expected.SetupGet(x => x.Name).Returns("Phoenix - V2");
            expected.SetupGet(x => x.Users).Returns(new List<User>().AsReadOnly());
            expected.SetupGet(x => x.Invites).Returns(new List<UserInvite>().AsReadOnly());
            expected.SetupGet(x => x.DateCreated).Returns(new DateTime(2015, 01, 25));
            expected.SetupGet(x => x.DateModified).Returns(new DateTime(2015, 01, 25));

            Guid tenantId = Guid.NewGuid();

            Group actual = Group.Create("Phoenix - V2");

            actual.ShouldBeEquivalentTo(expected.Object,
                config => config.Excluding(p => p.PropertyPath == "Id"));
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_NameIsMissing_ThrowsException()
        {
            Guid tenantId = Guid.NewGuid();
            Group.Create(String.Empty);
        }

        [TestMethod]
        public void Add_ValidUser_Added()
        {
            Mock<User> user = new Mock<User>();

            this.group.Add(user.Object);

            this.group.Users.Should().HaveCount(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Add_UserNull_ThrowsException()
        {
            this.group.Add(null as User);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Add_SameUser_ThrowsException()
        {
            Guid tenantId = Guid.NewGuid();

            User userA = User.Create("idpID", "userA@a.com", "A", false);

            User userAA = User.Create("idpID", "userA@a.com", "A", false);

            this.group.Add(userA);
            this.group.Add(userAA);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Remove_UserIsNull_ThrowsException()
        {
            this.group.Remove(null as User);
        }


        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Remove_UserIsNotInTheCollection_ThrowsException()
        {
            Mock<User> userA = new Mock<User>();
            Mock<User> userB = new Mock<User>();

            this.group.Add(userA.Object);
            this.group.Remove(userB.Object);
        }

        [TestMethod]
        public void Remove_UserIsInCollection_Removes()
        {
            Guid tenantId = Guid.NewGuid();

            User userA = User.Create("idpID", "userA@a.com", "A", false);

            this.group.Add(userA);
            this.group.Remove(userA);

            this.group.Users.Should().HaveCount(0);
        }

        [TestMethod]
        public void Add_ValidUserinvite_Added()
        {
            Mock<UserInvite> userInvite = new Mock<UserInvite>();

            this.group.Add(userInvite.Object);

            this.group.Invites.Should().HaveCount(1);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Add_UserInviteNull_ThrowsException()
        {
            this.group.Add(null as UserInvite);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Add_SameUserInvite_ThrowsException()
        {
            Guid userInvitedById = Guid.NewGuid();

            UserInvite userInviteA = UserInvite.Create(
                userInvitedById,
                "userA@a.com"
            );

            UserInvite userInviteAA = UserInvite.Create(
                userInvitedById,
                "userA@a.com"
            );

            this.group.Add(userInviteA);
            this.group.Add(userInviteAA);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Remove_UserInviteIsNull_ThrowsException()
        {
            this.group.Remove(null as UserInvite);
        }


        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Remove_UserInviteIsNotInTheCollection_ThrowsException()
        {
            Mock<UserInvite> userInviteA = new Mock<UserInvite>();
            Mock<UserInvite> userInviteB = new Mock<UserInvite>();

            this.group.Add(userInviteA.Object);
            this.group.Remove(userInviteB.Object);
        }

        [TestMethod]
        public void Remove_UserInviteIsInCollection_Removes()
        {
            Guid userInvitedById = Guid.NewGuid();

            UserInvite userInviteA = UserInvite.Create(
                userInvitedById,
                "userA@a.com"
            );

            this.group.Add(userInviteA);
            this.group.Remove(userInviteA);

            this.group.Invites.Should().HaveCount(0);
        }

    }
}
