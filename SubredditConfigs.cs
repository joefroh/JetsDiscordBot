namespace discordBot
{
    public class SubredditConfigs
    {
        public string ServerID { get; set; }
        public string ServerFriendlyName { get; set; }
        public string TargetChannelID { get; set; }
        public string TargetSubreddit { get; set; }
        public int NewSubmissionCacheSize { get; set; }
    }
}