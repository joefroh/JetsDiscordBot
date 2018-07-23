namespace NHLApi
{
    public class GameLiveData
    {
        public PlayData Plays { get; set; }
        public LineScore LineScore { get; set; }
        public BoxScore BoxScore { get; set; }
        public DecisionData Decisions { get; set; }
    }
}