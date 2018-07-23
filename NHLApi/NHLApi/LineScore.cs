using System.Collections.Generic;

namespace NHLApi
{
    public class LineScore
    {
        public int CurrentPeriod { get; set; }
        public string CurrentPeriodOrdinal { get; set; }
        public string CurrentPeriodTimeRemaining { get; set; }
        public List<LineScorePeriod> Periods { get; set; }
        public ShootoutData ShootoutInfo { get; set; }
        public LineScoreTeamData Team { get; set; }
        public string PowerPlayStrength { get; set; }
        public bool HasShootout { get; set; }
        public IntermissionData IntermissionInfo { get; set; }
        public PowerPlayData PowerPlayInfo { get; set; }
    }
}