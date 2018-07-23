namespace NHLApi
{
    public class TeamSkaterStats
    {
        public int Goals { get; set; }
        public int PIM { get; set; }
        public int Shots { get; set; }
        public float PowerPlayPercentage { get; set; }
        public float PowerPlayGoals { get; set; }
        public float PowerPlayOpportunities { get; set; }
        public float FaceOffWinPercentage { get; set; }
        public int Blocked { get; set; }
        public int Takeaways { get; set; }
        public int Giveaways { get; set; }
        public int Hits { get; set; }
    }
}