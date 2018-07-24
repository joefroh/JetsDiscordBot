using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHLApi
{
    public class MediaContent
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Blurb { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public bool AuthFlow { get; set; }
        public int MediaPlaybackId { get; set; }
        public string MediaState { get; set; }
        public List<KeyWord> KeyWords { get; set; }
        public ImageData Image { get; set; }
        public List<PlaybackData> Playbacks { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as MediaContent);
        }

        public bool Equals(MediaContent p)
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