namespace discordBot
{
    public class SubredditConfig
    {
        public ulong ServerID { get; set; }
        public string ServerFriendlyName { get; set; }
        public ulong TargetChannelID { get; set; }
        public string TargetSubreddit { get; set; }
        public int NewSubmissionCacheSize { get; set; }
    }
}