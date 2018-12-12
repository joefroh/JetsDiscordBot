using System;
using DiscordBot;

namespace DiscordBotTest.TestMocks
{
    public class TestEventHandler : IEventHandler
    {
        public bool HasFired = false;

        public Type Channel { get { return typeof(TestEvent); } }

        public void Fire(IEvent firedEvent)
        {
            HasFired = true;
        }
    }
}