using System;
using System.Threading.Tasks;
using DiscordBot;

namespace DiscordBotTest.TestMocks
{
    public class TestEventHandler : IEventHandler
    {
        public bool HasFired = false;

        public Type Channel { get { return typeof(TestEvent); } }

        public Task Fire(IEvent firedEvent)
        {
            HasFired = true;
            return null;
        }
    }
}