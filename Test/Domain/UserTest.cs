using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.Domain.Users;
using System;
using FluentAssertions;
using Server.Domain.Exceptions;

namespace Test.Domain
{
    [TestClass]
    public class UserTest
    {
        Mock<UserContact> userContact;
        User user;

        [TestInitialize]
        public void Init()
        {
            Guid tenantId = Guid.NewGuid();

            this.userContact = new Mock<UserContact>();
            this.userContact.SetupGet(x => x.Type).Returns(Contact.PrimaryNumber);

            this.user = User.Create("idpID", "bob.smith@test.com", "Bob Smith", false);
        }


        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ChangeFullName_EmptyString_ThrowsException()
        {
            this.user.ChangeFullName(String.Empty);
        }

        [TestMethod]
        public void ChangeFullName_FullNamePassed_Succesfull()
        {
            this.user.ChangeFullName("Dan");
            this.user.FullName.Should().Be("Dan");
        }

        [TestMethod]
        public void ChangeEmail_ValidEmailIsProvided_UserIsUpdated()
        {
            string email = "bob.smith@test.com";
            this.user.ChangeEmail(email);
            this.user.Email.Should().Be(email);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ChangeEmail_EmptyEmailIsProvided_ThrowsException()
        {
            this.user.ChangeEmail(String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void ChangeEmail_InvalidEmailIsProvided_ThrowsException()
        {
            this.user.ChangeEmail("bobsmith");
        }

        [TestMethod]
        public void ChangeManagerStatus_UserIsNotManager_SuccesfullyUpdates()
        {
            this.user.ChangeAdminRole(true);
            this.user.IsAdmin.Should().BeTrue();
        }

        [TestMethod]
        public void ChangeManagerStatus_UserIsManager_UserIsStillTheSame()
        {
            this.user.ChangeAdminRole(true);
            this.user.ChangeAdminRole(true);
            this.user.IsAdmin.Should().BeTrue();
        }
        
    }
}
