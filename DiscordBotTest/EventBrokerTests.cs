using System;
using discordBot;
using DiscordBotTest.TestMocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        [TestMethod]
        public void RegisterAndFireTrue()
        {
            var broker = new EventBroker();
            var testHandler = new TestEventHandler();

            broker.RegisterHandler(testHandler);
            broker.FireEvent(new TestEvent());

            Assert.IsTrue(testHandler.HasFired);
        }

        [TestMethod]
        public void RegisterAndFireFalse()
        {
            var broker = new EventBroker();
            var testHandler = new TestEventHandler();
            var testEvent = new TestEvent();
            testEvent.Channel = "foo";

            broker.RegisterHandler(testHandler);
            broker.FireEvent(testEvent);

            Assert.IsFalse(testHandler.HasFired);
        }
    }
}