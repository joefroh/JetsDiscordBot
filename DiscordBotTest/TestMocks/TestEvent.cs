using discordBot;

namespace DiscordBotTest.TestMocks {
    public class TestEvent : IEvent {
        string channel = "test";
        public TestEvent () {

        }

        public string Channel {
            get { return channel; }
            set {
                channel = value;
            }
        }
    }
}