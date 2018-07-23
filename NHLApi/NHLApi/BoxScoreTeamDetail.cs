using System.Collections.Generic;

namespace NHLApi
{
    public class BoxScoreTeamDetail
    {
        public TeamSimple Team { get; set; }
        public BoxScoreTeamStats TeamStats { get; set; }
        public dynamic Players { get; set; }   //TODO Players nonsense again
        public List<int> Goalies { get; set; }
        public List<int> Skaters { get; set; }
        public List<int> OnIce { get; set; }
        public List<OnIcePlayerStatus> OnIcePlus { get; set; }
        public List<int> Scratches { get; set; }
        public List<int> PenaltyBox { get; set; } //TODO Confirm this is correct
        public List<Coach> Coaches { get; set; }
    }
}