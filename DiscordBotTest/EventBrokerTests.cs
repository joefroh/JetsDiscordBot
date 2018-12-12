using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DiscordBotTest.TestMocks;
using discordBot;

namespace DiscordBotTest
{
    [TestClass]
    public class EventBrokerTests
    {
        [TestInitialize]
        public void TestInit()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DuplicateRegistration()
        {
            var broker = new EventBroker();
            var testHandler = new TestEventHandler();
            
            broker.RegisterHandler(testHandler);
            broker.RegisterHandler(testHandler);

            // This should have thrown an exception by now.
        }
    }
}
