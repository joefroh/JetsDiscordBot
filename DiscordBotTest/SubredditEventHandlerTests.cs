using System.Threading.Tasks;
using ClassLocator;
using Discord;
using DiscordBot;
using DiscordBotTest.TestMocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiscordBotTest
{
    [TestClass]
    public class SubredditEventHandlerTests
    {
        public TestDiscordClient Client { get; set; }
        public TestGuild Guild { get; set; }
        public TestTextChannel TextChannel { get; set; }
        public TestUserMessage Message { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Client = new TestDiscordClient();
            Guild = new TestGuild();
            TextChannel = new TestTextChannel();
            Message = new TestUserMessage();

            Locator.Instance.RegisterInstance<IDiscordClient>(Client);
            Locator.Instance.RegisterInstance<IGuild>(Guild);
            Locator.Instance.RegisterInstance<ITextChannel>(TextChannel);
            Locator.Instance.RegisterInstance<IUserMessage>(Message);
        }

        [TestMethod]
        public void FireSubredditEventAndTriggerSubredditEventHandler()
        {
            // Arrange
            ulong guildID = 321;
            ulong channelID = 123;
            var testMessage = new SubredditSubmission();
            testMessage.Message = "Test 123";
            testMessage.Id = "foobar"; // Note the submission cache requires an ID, this is ok.

            var broker = new EventBroker();
            var handler = new SubredditEventHandler();

            // Act
            broker.RegisterHandler(handler);
            var eventToFire = new SubredditEvent(guildID, channelID, testMessage);

            // Given this runs on a seperate thread, and we don't catch that exception, validate this ran properly
            Assert.IsTrue(broker.FireEvent(eventToFire).Status != TaskStatus.Faulted);

            // Assert
            Assert.AreEqual(guildID, Guild.ID);
            Assert.AreEqual(channelID, TextChannel.ID);
            Assert.AreEqual(testMessage.Message, Message.Message);
        }

        public void FireSubredditEventAndDelete()
        {
            // Arrange
            ulong guildID = 321;
            ulong channelID = 123;
            var testMessage = new SubredditSubmission();
            testMessage.Message = "Test 123";
            testMessage.Id = "deleteMe"; // Note the submission cache requires an ID, this is ok.

            var broker = new EventBroker();
            var handler = new SubredditEventHandler();

            // Act
            broker.RegisterHandler(handler);
            var eventToFire = new SubredditEvent(guildID, channelID, testMessage);
            var eventToDelete = new SubredditEvent(guildID, channelID, testMessage, true);

            // Given this runs on a seperate thread, and we don't catch that exception, validate this ran properly
            Assert.IsTrue(broker.FireEvent(eventToFire).Status != TaskStatus.Faulted);
            Assert.IsTrue(broker.FireEvent(eventToDelete).Status != TaskStatus.Faulted);

            // Assert
            Assert.IsTrue(Message.Deleted);
        }
    }
}