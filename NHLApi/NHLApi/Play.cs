using System.Collections.Generic;

namespace NHLApi
{
    public class Play
    {
        public PlayResult Result { get; set; }
        public PlayAbout About { get; set; }
        public PlayCoordinates Coordinates { get; set; }
        public List<PlayPlayer> Players { get; set; }
        public TeamSimple Team { get; set; }
    }
}