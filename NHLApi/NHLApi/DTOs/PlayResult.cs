using System;
using Newtonsoft.Json;

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

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PlayResult);
        }

        public bool Equals(PlayResult p)
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