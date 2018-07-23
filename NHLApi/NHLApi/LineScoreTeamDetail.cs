namespace NHLApi
{
    public class LineScoreTeamDetail
    {
        public TeamSimple Team { get; set; }
        public int Goals { get; set; }
        public int ShotsOnGoal { get; set; }
        public bool GoaliePulled { get; set; }
        public int NumSkaters { get; set; }
        public bool PowerPlay { get; set; }
    }
}