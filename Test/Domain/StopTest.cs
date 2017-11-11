using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using Moq;
using Server.Domain.Users;
using Server.Domain.Groups;
using Server.Domain.Stops;
using System.Collections.Generic;
using Server.Core.Time;

namespace Test.Domain
{
    [TestClass]
    public class StopTest
    {
        Mock<Group> group;
        User user;
        Stop stop;

        [TestInitialize]
        public void Init()
        {
            TimeProvider.Current = new TestTimeProvider(new DateTime(2015, 01, 25, 9, 0, 0));
            Guid tenantId = Guid.NewGuid();

            this.user =  User.Create("idpID", "bob@smith.com", "Bob Smith", false);

            this.group = new Mock<Group>();
            this.group.SetupGet(x => x.Users).Returns(new List<User>() { 
                    user
                }
                .AsReadOnly());

            this.stop = Stop.Create(this.group.Object, this.user, "why");
        }

        [TestMethod]
        public void Resolved_45MinutesLater_CorrectDowntimeProvided()
        {
            TimeProvider.Current = new TestTimeProvider(new DateTime(2015, 01, 25, 9, 45, 0));

            this.stop.ProblemResolved(this.user.Id);

            this.stop.OverallDownTime.Should().Be(new TimeSpan(0, 45, 0));
        }

    }
}
