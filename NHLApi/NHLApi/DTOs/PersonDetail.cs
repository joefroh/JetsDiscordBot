using System;
using Newtonsoft.Json;

namespace NHLApi
{
    public class PersonDetail
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Link { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PrimaryNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public int CurrentAge { get; set; }
        public string BirthCity { get; set; }
        public string BirthStateProvince { get; set; }
        public string BirthCountry { get; set; }
        public string Nationality { get; set; }
        public string Height { get; set; }
        public int Weight { get; set; }
        public bool Active { get; set; }
        public bool AlternativeCaptain { get; set; }
        public bool Captain { get; set; }
        public bool Rookie { get; set; }
        public string ShootsCatches { get; set; }
        public string RosterStatus { get; set; }
        public TeamSimple CurrentTeam { get; set; }
        public Position PrimaryPosition { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PersonDetail);
        }

        public bool Equals(PersonDetail p)
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