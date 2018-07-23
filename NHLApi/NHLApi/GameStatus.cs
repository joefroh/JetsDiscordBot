namespace NHLApi
{
    public class GameStatus
    {
        public string AbstractGameState { get; set; }
        public int CodedGameState { get; set; }
        public string DetailedState { get; set; }
        public int StatusCode { get; set; }
        public bool StartTimeTBD { get; set; }
    }
}