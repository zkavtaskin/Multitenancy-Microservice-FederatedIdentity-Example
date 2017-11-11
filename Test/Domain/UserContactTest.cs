using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Domain.Users;
using Moq;
using FluentAssertions;
using Server.Domain.Exceptions;

namespace Test.Domain
{
    [TestClass]
    public class UserContactTest
    {
        [TestMethod]
        public void Create_AllInformationProvided_Success()
        {
            Mock<User> user = new Mock<User>();

            Mock<UserContact> expected = new Mock<UserContact>();
            expected.SetupGet(x => x.Type).Returns(Contact.PrimaryNumber);
            expected.SetupGet(x => x.Value).Returns("000");

            UserContact actual = UserContact.Create(user.Object, Contact.PrimaryNumber, "000");

            actual.ShouldBeEquivalentTo(expected.Object, opt =>
                opt.Excluding(
                    prop =>
                        prop.PropertyPath == "Id"
                    ));
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_ValueInformationIsMissing_ThrowsException()
        {
            Mock<User> user = new Mock<User>();

            UserContact.Create(user.Object, Contact.PrimaryNumber, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void Create_UserIsNull_ThrowsException()
        {
            UserContact.Create(null, Contact.PrimaryNumber, "000");
        }
    }
}
