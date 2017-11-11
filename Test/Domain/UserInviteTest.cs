using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Domain.Users;
using FluentAssertions;
using Moq;
using Server.Domain.Tenants;
using Server.Domain.Exceptions;
using Server.Core.Time;
using Server.Domain.Groups;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Test.Domain
{
    [TestClass]
    public class UserInviteTest
    {
        Guid tenantId;
        Guid userInvitedById;
        Mock<User> user;

        [TestInitialize]
        public void Init()
        {
            this.tenantId = Guid.NewGuid();
            this.userInvitedById = Guid.NewGuid();

            TimeProvider.Current = new TestTimeProvider(new DateTime(2015, 01, 01));

            this.user = new Mock<User>();
        }

        [TestMethod]
        public void Create_EmailIsProvided()
        {
            Mock<UserInvite> expected = new Mock<UserInvite>();
            expected.SetupGet(x => x.Email).Returns("bob@smith.com");
            expected.SetupGet(x => x.DateCreated).Returns(TimeProvider.Current.UtcNow);
            expected.SetupGet(x => x.DateModified).Returns(TimeProvider.Current.UtcNow);
            expected.SetupGet(x => x.GroupIds).Returns(new List<Guid>().AsReadOnly());
            expected.SetupGet(x => x.InvitedByUserId).Returns(this.userInvitedById);

            UserInvite actual = UserInvite.Create(this.userInvitedById, "bob@smith.com");
            actual.ShouldBeEquivalentTo(expected.Object, opt => 
                opt.Excluding(
                    prop =>
                        prop.PropertyPath == "Id" ||
                        prop.PropertyPath == "InviteKey"
                    ));
        }


        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_TenantIsNull_Throws()
        {
            UserInvite.Create(this.userInvitedById, String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_UserIsNull_Throws()
        {
            UserInvite.Create(Guid.Empty, String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_EmailIsInvalid_Throws()
        {
            UserInvite.Create(this.userInvitedById, "bobsmith.com");
        }

        [TestMethod]
        public void Add_UserIsAddedToTheGroup()
        {
            Mock<Group> group = new Mock<Group>();
            group.SetupGet(x => x.Id).Returns(Guid.Empty);

            UserInvite userInvite = UserInvite.Create(this.userInvitedById, "bob@smith.com");
            userInvite.Add(group.Object);

            userInvite.GroupIds[0].Should().Be(Guid.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Add_UserIsAlreadyAddedToTheGroup_Throws()
        {
            Mock<Group> group = new Mock<Group>();
            group.SetupGet(x => x.Id).Returns(Guid.Empty);

            UserInvite userInvite = UserInvite.Create(this.userInvitedById, "bob@smith.com");
            userInvite.Add(group.Object);
            userInvite.Add(group.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Add_GrouIsNull_Throws()
        {
            Mock<Group> group = new Mock<Group>();
            group.SetupGet(x => x.Id).Returns(Guid.Empty);

            UserInvite userInvite = UserInvite.Create(this.userInvitedById, "bob@smith.com");
            userInvite.Add(null);
        }


        [TestMethod]
        public void Remove_UserIsRemovedFromTheGroup()
        {
            Mock<Group> group = new Mock<Group>();
            group.SetupGet(x => x.Id).Returns(Guid.Empty);

            UserInvite userInvite = UserInvite.Create(this.userInvitedById, "bob@smith.com");
            userInvite.Add(group.Object);
            userInvite.Remove(group.Object);

            userInvite.GroupIds.Should().HaveCount(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Remove_UserWasNotInTheGroup_Throws()
        {
            Mock<Group> group = new Mock<Group>();
            group.SetupGet(x => x.Id).Returns(Guid.Empty);

            UserInvite userInvite = UserInvite.Create(this.userInvitedById, "bob@smith.com");
            userInvite.Remove(group.Object);
        }

        [TestMethod]
        public void ResendInvite_KeyIsRegenerated()
        {
            UserInvite userInvite = UserInvite.Create(this.userInvitedById, "bob@smith.com");
            Guid inviteKeyCurrent = userInvite.InviteKey;
            userInvite.ResendInvite();
            inviteKeyCurrent.Should().NotBe(userInvite.InviteKey);
        }
    }
}
