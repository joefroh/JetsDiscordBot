using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHLApi
{
    public class GameCenterData
    {
        public string Title { get; set; }
        public int TopicList { get; set; }
        public List<MediaContent> Items { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as GameCenterData);
        }

        public bool Equals(GameCenterData p)
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