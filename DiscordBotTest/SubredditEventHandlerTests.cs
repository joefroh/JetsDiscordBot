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
            var testMessage = "This is a test message";
            var broker = new EventBroker();
            var handler = new SubredditEventHandler();
            
            // Act
            broker.RegisterHandler(handler);
            var eventToFire = new SubredditEvent(guildID, channelID, testMessage);

            // Given this runs on a seperate thread, and we don't catch that exception, validate this ran properly
            Assert.IsTrue(broker.FireEvent(eventToFire).Status != TaskStatus.Faulted);

            // Assert
            Assert.AreEqual(guildID, Guild.ID);
            Assert.AreEqual(channelID,TextChannel.ID);
            Assert.AreEqual(testMessage, Message.Message);
        }
    }
}