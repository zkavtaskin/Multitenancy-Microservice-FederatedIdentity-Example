using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Domain.Exceptions;
using Moq;
using Server.Domain.Users;
using FluentAssertions;

namespace Test.Domain
{
    [TestClass]
    public class User_ContactsManagement_Test
    {
        Mock<UserContact> userContact;
        User user;

        [TestInitialize]
        public void Init()
        {
            Guid tenantId = Guid.NewGuid();

            this.userContact = new Mock<UserContact>();
            this.userContact.SetupGet(x => x.Type).Returns(Contact.PrimaryNumber);

            this.user = User.Create("idpID", "bob.smith@test.com", "Bob Smith",  true);
        }

        [TestMethod]
        public void GetContacts_NoContacts_ReturnsEmptyCollection()
        {
            this.user.Contacts.Should().BeEmpty();
        }

        [TestMethod]
        public void Add_UniqueContact_Success()
        {
            this.user.Add(userContact.Object);

            user.Contacts.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Add_NullContact_ThrowsException()
        {
            this.user.Add(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Add_DuplicateContactWithSameContactType_ThrowsException()
        {
            this.user.Add(UserContact.Create(this.user, Contact.PrimaryNumber, "000"));
            this.user.Add(UserContact.Create(this.user, Contact.PrimaryNumber, "000"));
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Remove_NullContact_ThrowsException()
        {
            this.user.Remove(null);
        }

        [TestMethod]
        public void Remove_ContactWithSameContactType_Success()
        {
            this.user.Add(UserContact.Create(this.user, Contact.PrimaryNumber, "000"));
            this.user.Remove(UserContact.Create(this.user, Contact.PrimaryNumber, "000"));

            this.user.Contacts.Should().BeEmpty();
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Remove_UnexistingContact_ThrowsException()
        {
            this.user.Remove(this.userContact.Object);
        }

    }
}
