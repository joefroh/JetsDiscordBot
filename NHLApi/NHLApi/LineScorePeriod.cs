using System;

namespace NHLApi
{
    public class LineScorePeriod
    {
        public string PeriodType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Num { get; set; }
        public string OrdinalNum { get; set; }
        public LinePeriodSummary Home { get; set; }
        public LinePeriodSummary Away { get; set; }
    }
}