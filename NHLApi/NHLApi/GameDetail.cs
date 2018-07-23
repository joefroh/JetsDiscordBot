using System;

namespace NHLApi
{
    public class GameDetail
    {
        public string Link { get; set; }
        public int GamePK { get; set; }
        public GameMetaData MetaData { get; set; }
        public GameData GameData { get; set; }
        public GameLiveData LiveData { get; set; }
    }
}
