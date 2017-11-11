using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Server.Core.Message;
using FluentAssertions;

namespace Test.Core
{
    [TestClass]
    public class MessageTest
    {
        [TestMethod]
        public void TokenGenerator_Generate_PassedAnonymousObj_TokensGenerated()
        {
            List<KeyValuePair<string, string>> expected = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("@test1@", "value1"),
                new KeyValuePair<string, string>("@test2@", "value2"),
            };

            TokenGenerator tokenGenerator = new TokenGenerator();

            List<KeyValuePair<string, string>> actual = tokenGenerator.Generate(new 
            {
                test1 = "value1",
                test2 = "value2"
            });

            actual.ShouldBeEquivalentTo(expected);
        }


    }
}
