using System.Collections.Generic;

namespace discordBot
{
    public class ReactionActorConfig
    {
        public string Trigger { get; set; }
        public string Reaction { get; set; }
        public List<string> Ignore { get; set; }
    }
}