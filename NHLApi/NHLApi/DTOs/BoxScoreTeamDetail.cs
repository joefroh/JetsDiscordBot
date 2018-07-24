using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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

        public override bool Equals(object obj)
        {
            return this.Equals(obj as BoxScoreTeamDetail);
        }

        public bool Equals(BoxScoreTeamDetail p)
        {
            // If parameter is null, return false.
            if (Object.ReferenceEquals(p, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, p))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != p.GetType())
            {
                return false;
            }

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return JsonConvert.SerializeObject(this) == JsonConvert.SerializeObject(p);
        }

        public override int GetHashCode()
        {
            return JsonConvert.SerializeObject(this).GetHashCode();
        }
    }
}