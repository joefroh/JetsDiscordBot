namespace NHLApi
{
    public class PlayResult
    {
        public string Event { get; set; }
        public string EventCode { get; set; }
        public string EventTypeId { get; set; }
        public string Description { get; set; }
        public string SecondaryType { get; set; }
        public PlayStrength Strength { get; set; }
        public bool GameWinningGoal { get; set; }
        public bool EmptyNet { get; set; }
    }
}