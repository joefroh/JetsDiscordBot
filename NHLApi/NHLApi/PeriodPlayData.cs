using System.Collections.Generic;

namespace NHLApi
{
    public class PeriodPlayData
    {
        public int StartIndex { get; set; }
        public List<int> Plays { get; set; }
        public int EndIndex { get; set; }
    }
}