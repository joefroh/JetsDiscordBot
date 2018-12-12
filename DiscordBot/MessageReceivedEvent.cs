using System;

namespace DiscordBot {
    public class MessageReceivedEvent : IEvent {
        public Type Channel { get { return this.GetType (); } }
    }
}