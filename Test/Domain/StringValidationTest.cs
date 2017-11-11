using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using FluentAssertions;
using Server.Domain;
using System.Collections.Generic;

namespace Test.Domain
{
    [TestClass]
    public class StringValidationTest
    {
        [TestMethod]
        public void EmailsValid_AllShouldPass()
        {
            List<string> emailsValid = new List<string>() {
                "bob@smith.com",
                "bob.smith@gmail.com",
                "b.b@gmail.com",
                "b@gmail.co.uk",
                "b@b.com",
                "b@b.co.uk",
                "b@b.co",
                "eden.o'smith@smith.com",
                "rrrrrrr.2311sasdf@daaasssssssssssssssssssssssssssssssssssaaaaa.co.uk",
                "bob+smith@smith.com"
            };

            foreach(string email in emailsValid)
            {
                Regex.IsMatch(email, StringValidation.Email).Should().BeTrue(String.Format("{0} is valid", email));
            }
        }


        [TestMethod]
        public void EmailIsNotValid_AllShouldPass()
        {
            List<string> emailNotValid = new List<string>() {
                "bob@smithcom",
                "bob.smith-gmail.com",
                "gmail.com",
                ".co.uk",
                "b@b",
                "@",
                ".co"
            };


            foreach (string email in emailNotValid)
            {
                Regex.IsMatch(email, StringValidation.Email).Should().BeFalse(String.Format("{0} is not valid", email));
            }
        }

        [TestMethod]
        public void TenantName_AllShouldPass()
        {
            List<string> tenantNames = new List<string>() {
                "Orange",
                "Single Space",
                "G00gle",
                "Some Company Name Ltd"
            };

            foreach (string tenantName in tenantNames)
            {
                Regex.IsMatch(tenantName, StringValidation.TenantName).Should().BeTrue(String.Format("{0} is valid", tenantName));
            }
        }

        [TestMethod]
        public void TenantName_AllShouldFail()
        {
            List<string> tenantNames = new List<string>() {
                "Some^Company",
                "_Orange23",
                "+0range",
                "HTC_",
                "_HTC_",
                "!£$%^&*()"
            };

            foreach (string tenantName in tenantNames)
            {
                Regex.IsMatch(tenantName, StringValidation.TenantName).Should().BeFalse(String.Format("{0} is not valid", tenantName));
            }
        }

        [TestMethod]
        public void TenantNameFriendly_AllShouldPass()
        {
            List<string> tenantNames = new List<string>() {
                "somecompany",
                "some_comapny",
                "23_some_company",
                "_somecompany",
                "a"
            };

            foreach (string tenantName in tenantNames)
            {
                Regex.IsMatch(tenantName, StringValidation.TenantNameFriendly).Should().BeTrue(String.Format("{0} is valid", tenantName));
            }
        }

        [TestMethod]
        public void TenantNameFriendly_AllShouldFail()
        {
            List<string> tenantNames = new List<string>() {
                "SomeCompany",
                "Some Company",
                "some company",
                "somecompany^",
                String.Empty,
                "!"
            };

            foreach (string tenantName in tenantNames)
            {
                Regex.IsMatch(tenantName, StringValidation.TenantNameFriendly).Should().BeFalse(String.Format("{0} is not valid", tenantName));
            }
        }
    }
}
