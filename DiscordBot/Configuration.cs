using System.Collections.Generic;

namespace discordBot
{
    public class Configuration
    {
        [ConfigurationRequired]
        public string Token { get; set; }
        public bool EnableTextCommands { get; set; }

        [ConfigurationRequired]
        public string CommandPrefix { get; set; }
        public int RedditRefreshTimer { get; set; }

        [ConfigurationRequired]
        public ulong AdminServerID { get; set; }

        [ConfigurationRequired]
        public ulong AdminChannelID { get; set; }
        public IEnumerable<SubredditConfig> SubredditConfig { get; set; }
        public IEnumerable<GoalHornConfig> GoalHornConfig { get; set; }
        public IEnumerable<ReactionActorConfig> ReactionActorConfig { get; set; }
        public bool EnablePollers { get; set; }
        public bool EnableReactions { get; set; }
        public string LogFile { get; set; }
    }
}