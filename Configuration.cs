using System.Collections.Generic;

namespace discordBot
{
    public class Configuration
    {
        public string Token { get; set; }
        public bool EnableCommands { get; set; }
        public string CommandPrefix { get; set; }
        public int RedditRefreshTimer { get; set; }
        public IEnumerable<SubredditConfig> SubredditConfig { get; set; }
    }
}