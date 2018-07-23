using System;

namespace NHLApi
{
    public class PlayAbout
    {
        public int EventIdx { get; set; }
        public int EventId { get; set; }
        public string PeriodType { get; set; }
        public string OrdinalNum { get; set; }
        public string PeriodTime { get; set; }
        public string PeriodTimeRemaining { get; set; }
        public DateTime DateTime { get; set; }
        public PlayGoals Goals { get; set; }
    }
}