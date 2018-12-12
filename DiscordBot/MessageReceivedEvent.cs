using System;
using Discord.WebSocket;

namespace DiscordBot
{
    public class MessageReceivedEvent : IEvent
    {

        public Type Channel { get { return this.GetType(); } }
        public SocketUserMessage Message { get; private set; }

        public MessageReceivedEvent(SocketUserMessage msg)
        {
            Message = msg;
        }
    }
}