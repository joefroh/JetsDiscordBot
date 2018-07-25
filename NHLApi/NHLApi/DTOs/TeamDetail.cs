using System;
using Newtonsoft.Json;

namespace NHLApi
{
    public class TeamDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public Venue Venue { get; set; }
        public string Abbreviation { get; set; }
        public string TeamName { get; set; }
        public string LocationName { get; set; }
        public int FirstYearOfPlay { get; set; }
        public Division Division { get; set; }
        public Conference Conference { get; set; }
        public Franchise Franchise { get; set; }
        public GameScheduleData NextGameSchedule { get; set; }
        public GameScheduleData PreviousGameSchedule { get; set; }
        public string ShortName { get; set; }
        public string OfficialSiteUrl { get; set; }
        public int FranchiseId { get; set; }
        public bool Active { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as TeamDetail);
        }

        public bool Equals(TeamDetail p)
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