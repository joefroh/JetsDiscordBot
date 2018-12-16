using System;

namespace DiscordBot
{
    public class SubredditEvent : IEvent
    {
        public Type Channel { get { return this.GetType(); } }
        public ulong Server { get; set; }
        public ulong ServerChannel { get; set; }
        public SubredditSubmission Submission { get; set; }
        public string PostId { get; set; }
        public bool IsRemoval { get; set; }


        public SubredditEvent(ulong server, ulong channel, SubredditSubmission submission)
        {
            Server = server;
            ServerChannel = channel;
            Submission = submission;
            IsRemoval = false;
        }
        public SubredditEvent(ulong server, ulong channel, SubredditSubmission submission, bool isRemoval): this(server, channel, submission)
        {
            IsRemoval = isRemoval;
        }
    }
}