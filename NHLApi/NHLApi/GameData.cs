namespace NHLApi
{
    public class GameData
    {
        public GameSimple Game { get; set; }
        public GameDateRange DateTime { get; set; }
        public GameStatus Status { get; set; }
        public GameTeams Teams { get; set; }
        public dynamic Players { get; set; }   //TODO This will get weird.
        public VenueSimple Venue { get; set; }
    }
}