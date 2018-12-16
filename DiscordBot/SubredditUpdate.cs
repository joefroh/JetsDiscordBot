using System.Collections.Generic;

namespace DiscordBot
{
    public class SubredditUpdate
    {
        public IEnumerable<SubredditSubmission> NewSubmissions { get; set; }
        public IEnumerable<SubredditSubmission> RemovedSubmissions {get; set;}
    }
}