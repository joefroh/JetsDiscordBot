using discordBot;

namespace DiscordBotTest.TestMocks
{
    public class TestEventHandler : IEventHandler
    {
        public string ChannelString { get { return "test"; } }
    }
}