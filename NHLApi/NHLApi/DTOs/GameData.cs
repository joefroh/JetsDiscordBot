using System;
using Newtonsoft.Json;

namespace NHLApi
{
    public class GameData
    {
        public GameSimple Game { get; set; }
        public GameDateRange DateTime { get; set; }
        public GameStatus Status { get; set; }
        public GameTeams Teams { get; set; }
        public dynamic Players { get; set; }
        public VenueSimple Venue { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as GameData);
        }

        public bool Equals(GameData p)
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