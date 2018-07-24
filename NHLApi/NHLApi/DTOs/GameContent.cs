using System;
using Newtonsoft.Json;

namespace NHLApi
{
    // Note for this class, I have deliberately not implemented the full Editorial and Media content because I'm not using this.
    // If you are consuming this library and this bugs you, feel free to open a PR doing this.

    public class GameContent
    {
        public dynamic Editorial { get; set; }
        public dynamic Media { get; set; }

        public HighlightData Highlights { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as GameContent);
        }

        public bool Equals(GameContent p)
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