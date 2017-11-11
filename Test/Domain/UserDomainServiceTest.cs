using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Moq;
using Server.Core.Repository;
using Server.Domain.Tenants;
using System.Collections.Generic;
using Server.Domain.Users;
using Server.Domain.Groups;
using Server.Domain.Stops;
using System.Linq.Expressions;
using Server.Domain.Exceptions;

namespace Test.Domain
{
    [TestClass]
    public class UserDomainServiceTest
    {
        Mock<IRepository<User>> userRepo;
        Mock<IRepository<UserInvite>> userInviteRepo;
        Mock<IRepository<Group>> groupRepo;
        Mock<IRepository<Stop>> stopRepo;
        Guid tenantId;
        UserDomainService userDomainService;

        [TestInitialize]
        public void Init()
        {
            this.userRepo = new Mock<IRepository<User>>();
            this.userInviteRepo = new Mock<IRepository<UserInvite>>();
            this.groupRepo = new Mock<IRepository<Group>>();
            this.stopRepo = new Mock<IRepository<Stop>>();
            this.tenantId = Guid.NewGuid();

            this.userDomainService =
                new UserDomainService(this.userRepo.Object, this.userInviteRepo.Object, this.groupRepo.Object, this.stopRepo.Object);
        }

        [TestMethod]
        public void Add_UserIsInvited_UserAdded()
        {
            Mock<User> user = new Mock<User>();
            user.SetupGet(x => x.Email).Returns("bob@test.com");

            Mock<UserInvite> userInvite = new Mock<UserInvite>();
            userInvite.SetupGet(x => x.Email).Returns("bob@test.com");

            this.userInviteRepo.Setup(x => x.FindOne(It.IsAny<Expression<Func<UserInvite, bool>>>()))
                .Returns(userInvite.Object);

            this.userDomainService.Add(user.Object);

            this.userRepo.Verify(x => x.Add(user.Object));
        }

        [TestMethod]
        public void Add_UserIsNotFistUserInviteIsInGroups_UserAddedToGroup()
        {
            Guid groupIdA = Guid.NewGuid();

            Mock<Group> groupA = new Mock<Group>();
            groupA.SetupGet(x => x.Id).Returns(groupIdA);
            groupA.Setup(x => x.Add(It.IsAny<User>()));

            Mock<User> user = new Mock<User>();
            user.SetupGet(x => x.Email).Returns("bob@smith.com");

            Mock<UserInvite> userInvite = new Mock<UserInvite>();
            userInvite.SetupGet(x => x.Email).Returns("bob@smith.com");
            userInvite.SetupGet(x => x.GroupIds).Returns(
                new List<Guid>()
                {
                    groupIdA
                }.AsReadOnly());


            this.userRepo.Setup(x => x.Count()).Returns(1);

            this.userInviteRepo.Setup(x => x.FindOne(It.IsAny<Expression<Func<UserInvite, bool>>>()))
                .Returns(userInvite.Object);

            this.groupRepo.Setup(x => x.Find(It.IsAny<Expression<Func<Group, bool>>>()))
                .Returns(new List<Group>() { groupA.Object });

            this.userDomainService.Add(user.Object);

            groupA.Verify(x => x.Add(user.Object), Times.Once());
        }

        [TestMethod]
        public void Add_UserIsNotFirstUserIsInvited_UserInviteIsRemoved()
        {
            Mock<User> user = new Mock<User>();
            user.SetupGet(x => x.Email).Returns("bob@smith.com");

            Mock<UserInvite> userInvite = new Mock<UserInvite>();
            userInvite.SetupGet(x => x.Email).Returns("bob@smith.com");

            this.userRepo.Setup(x => x.Count()).Returns(1);

            this.userInviteRepo.Setup(x => x.FindOne(It.IsAny<Expression<Func<UserInvite, bool>>>()))
                .Returns(userInvite.Object);

            this.userDomainService.Add(user.Object);

            this.userInviteRepo.Verify(x => x.Remove(userInvite.Object));
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Add_UserIsNotFirstAndUserIsNotInvited_Throws()
        {
            Mock<User> user = new Mock<User>();
            user.SetupGet(x => x.Email).Returns("bob@smith.com");

            this.userRepo.Setup(x => x.Count()).Returns(1);

            this.userDomainService.Add(user.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Add_UserIsNotFirstTwoUsersWithSameEmail_ThrowsException()
        {
            Mock<User> user = new Mock<User>();
            user.SetupGet(x => x.Email).Returns("bob@smith.com");

            this.userRepo.Setup(x => x.Count()).Returns(1);

            this.userRepo.Setup(x => x.FindOne(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(user.Object);

            this.userDomainService.Add(user.Object);
        }

  
        [TestMethod]
        public void Remove_ExistingUser_SuccesfullyGetsRemoved()
        {
            Mock<User> user = new Mock<User>();
            user.SetupGet(x => x.Id).Returns(Guid.NewGuid());
            user.SetupGet(x => x.Email).Returns("bob@smith.com");

            this.userRepo.Setup(x => x.FindById(It.IsAny<Guid>()))
                .Returns(user.Object);

            this.userDomainService.Remove(user.Object.Id);

            this.userRepo.Verify(x => x.Remove(user.Object));
        }

       
        [TestMethod]
        public void Remove_ExistingUser_SuccesfullyGetsRemovedFromGroups()
        {
            Mock<User> user = new Mock<User>();
            user.SetupGet(x => x.Id).Returns(Guid.NewGuid());
            user.SetupGet(x => x.Email).Returns("bob@smith.com");

            Mock<Group> group = new Mock<Group>();
            group.SetupGet(x => x.Id).Returns(Guid.NewGuid());
            group.SetupGet(x => x.Users).Returns(new List<User>() { user.Object }.AsReadOnly());

            this.userRepo.Setup(x => x.FindById(It.IsAny<Guid>()))
                .Returns(user.Object);

            this.groupRepo.Setup(x => x.Find(It.IsAny<Expression<Func<Group, bool>>>()))
                .Returns(new List<Group>() { group.Object });

            this.userDomainService.Remove(user.Object.Id);

            group.Verify(x => x.Remove(user.Object), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Remove_UserDoesntExist_ThrowException()
        {
            this.userDomainService.Remove(Guid.NewGuid());
        }

 
        [TestMethod]
        public void Add_UserInviteIsBeingAdded_UserInviteIsAdded()
        {
            Mock<Group> group = new Mock<Group>();

            this.groupRepo.Setup(x => x.FindById(It.IsAny<Guid>()))
                .Returns(group.Object);

            this.userDomainService.InviteUser(Guid.NewGuid(), Guid.NewGuid(), "bob@smith.com");

            this.userInviteRepo.Verify(x => x.Add(It.IsAny<UserInvite>()));
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Add_UserInviteIsBeingAddedButUserAlreadyExists_Throws()
        {
            Mock<Group> group = new Mock<Group>();
            this.groupRepo.Setup(x => x.FindById(It.IsAny<Guid>()))
                .Returns(group.Object);

            Mock<User> user = new Mock<User>();

            this.userRepo.Setup(x => x.Find(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(new List<User>() { user.Object });

            this.userDomainService.InviteUser(Guid.NewGuid(), Guid.NewGuid(), "bob@smith.com");
        }

        [TestMethod]
        public void ChangeAdminRole_UserIsNotAdmin_ChangeToAdminIsSuccessful()
        {
            User actual = User.Create("idpid", "email@email.com", "fullname", false);
            Guid userId = Guid.NewGuid();
            this.userRepo.Setup(x => x.FindById(userId)).Returns(actual);
            this.userDomainService.ChangeAdminRole(userId, true);

            Assert.AreEqual(true, actual.IsAdmin);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ChangeAdminRole_UserIsAdminThereAreNoOtherAmins_RemoveOfAdminRoleIsNotSuccessful()
        {
            User actual = User.Create("idpid", "email@email.com", "fullname", false);
            Guid userId = Guid.NewGuid();
            this.userRepo.Setup(x => x.FindById(userId)).Returns(actual);
            this.userDomainService.ChangeAdminRole(userId, false);
        }

        [TestMethod]
        public void ChangeAdminRole_UserIsAdminThereAre2OtherAdmins_RemoveOfAdminRoleIsSuccessful()
        {
            User actual = User.Create("idpid", "email@email.com", "fullname", false);
            Guid userId = Guid.NewGuid();
            this.userRepo.Setup(userRepoProps => userRepoProps.FindById(userId)).Returns(actual);
            this.userRepo.Setup(userRepoProps => userRepoProps.Count(users => users.IsAdmin)).Returns(2);
            this.userDomainService.ChangeAdminRole(userId, false);

            Assert.AreEqual(false, actual.IsAdmin);
        }

    }
}
