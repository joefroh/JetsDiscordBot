using Discord.WebSocket;

namespace discordBot
{
    public abstract class IPoller
    {
        protected DiscordSocketClient _client;
        protected Configuration _config;
        protected IPoller(DiscordSocketClient client, Configuration config)
        {
            _client = client;
            _config = config;
        }
        public abstract void StartPoll();
    }
}