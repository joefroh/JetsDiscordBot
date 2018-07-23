using System.Collections.Generic;

namespace NHLApi
{
    public class PlayData
    {
        public List<Play> AllPlays { get; set; }
        public List<int> ScoringPlays { get; set; }
        public List<int> PenaltyPlays { get; set; }
        public List<PeriodPlayData> PlaysByPeriod { get; set; }
        public Play CurrentPlay { get; set; }
    }
}