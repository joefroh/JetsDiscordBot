using DiscordBot;

namespace DiscordBotTest.TestMocks
{
    public class TestEventHandler : IEventHandler
    {
        public bool HasFired = false;
        public string ChannelString { get { return "test"; } }

        public void Fire(IEvent firedEvent)
        {
            HasFired = true;
        }
    }
}