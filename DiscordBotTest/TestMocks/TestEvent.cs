using System;
using DiscordBot;

namespace DiscordBotTest.TestMocks {
    public class TestEvent : IEvent {
        Type channel;
        public TestEvent () {
            channel = this.GetType ();
        }

        public Type Channel {
            get { return channel; }
            set {
                channel = value;
            }
        }
    }
}