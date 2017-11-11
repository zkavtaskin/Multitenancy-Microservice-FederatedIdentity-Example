using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Core.Message.Email;
using System.Collections.Generic;
using FluentAssertions;

namespace Test.Core
{
    [TestClass]
    public class EmailMessageTest
    {
        [TestMethod]
        public void EmailMessageFormater_FormatTemplate()
        {
            EmailMessage expected = new EmailMessage();
            expected.Subject = "Mr Bob Smith, email from your provider";
            expected.Body = "Dear Mr Bob Smith, we have an email for you";
            expected.BodyPlainText = "Dear Bob Smith, email for you";

            EmailTemplate emailTemplate = new EmailTemplate();
            emailTemplate.Subject = "@title @fullname, email from your provider";
            emailTemplate.BodyHtml = "Dear @title @fullname, we have an email for you";
            emailTemplate.BodyText = "Dear @fullname, email for you";

            List<KeyValuePair<string, string>> tokensWithValues = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("@title", "Mr"),
                new KeyValuePair<string, string>("@fullname", "Bob Smith")
            };

            EmailMessageFormater msgFormater = new EmailMessageFormater();
            EmailMessage actual = msgFormater.Format(emailTemplate, tokensWithValues);

            actual.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        public void MessageTemplateFactory_CreatesEmailTemplate()
        {
            EmailTemplate expected = new EmailTemplate();
            expected.Subject = "subject test";
            expected.BodyHtml = "html body test";
            expected.BodyText = "text body test";

            MessageTemplateFactory<EmailTemplate> templateFactory = new MessageTemplateFactory<EmailTemplate>();
            EmailTemplate actual = templateFactory.Create(EmailTemplates.ResourceManager, "Test");

            actual.ShouldBeEquivalentTo(expected);
        }

    }
}
