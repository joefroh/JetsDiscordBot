using System;
using Newtonsoft.Json;

namespace NHLApi
{
    public class ImageCutData
    {
        [JsonProperty("1136x640")]
        public ImageDetail Size1136x640 { get; set; }
        [JsonProperty("1024x576")]
        public ImageDetail Size1024x576 { get; set; }
        [JsonProperty("960x540")]
        public ImageDetail Size960x540 { get; set; }
        [JsonProperty("768x432")]
        public ImageDetail Size768x432 { get; set; }
        [JsonProperty("640x360")]
        public ImageDetail Size640x360 { get; set; }
        [JsonProperty("568x320")]
        public ImageDetail Size568x320 { get; set; }
        [JsonProperty("372x210")]
        public ImageDetail Size372x210 { get; set; }
        [JsonProperty("320x180")]
        public ImageDetail Size320x180 { get; set; }
        [JsonProperty("248x140")]
        public ImageDetail Size248x140 { get; set; }
        [JsonProperty("124x70")]
        public ImageDetail Size124x70 { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ImageCutData);
        }

        public bool Equals(ImageCutData p)
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