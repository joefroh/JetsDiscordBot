using System.Collections.Generic;

namespace NHLApi
{
    public class BoxScore
    {
        public BoxScoreTeamData Teams { get; set; }
        public List<OfficialPerson> Officials { get; set; }
    }
}