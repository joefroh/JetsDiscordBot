namespace NHLApi
{
    public class Division
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Abbreviation { get; set; }
        public Conference Conference { get; set; }
        public bool Active { get; set; }
    }
}