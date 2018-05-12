namespace discordBot
{
    public class GoalHornConfig
    {
        public int Team { get; set; }
        public string TeamFriendlyName { get; set; }
        public ulong ServerID { get; set; }
        public string ServerFriendlyName { get; set; }
        public ulong TargetChannelID { get; set; }
        public int Delay { get; set; }
        public string PreText { get; set; }
        public string PostText { get; set; }
    }
}