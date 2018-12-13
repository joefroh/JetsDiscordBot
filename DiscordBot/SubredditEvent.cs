using System;

namespace DiscordBot
{
    public class SubredditEvent : IEvent
    {
        public Type Channel { get { return this.GetType(); } }
        public ulong Server { get; set; }
        public ulong ServerChannel { get; set; }
        public string Submission { get; set; }


        public SubredditEvent(ulong server, ulong channel, string submission)
        {
            Server = server;
            ServerChannel = channel;
            Submission = submission;
        }
    }
}