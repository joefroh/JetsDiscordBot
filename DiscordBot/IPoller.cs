using Discord.WebSocket;

namespace discordBot
{
    public abstract class IPoller
    {
        protected DiscordSocketClient _client;
        protected IPoller(DiscordSocketClient client)
        {
            _client = client;
        }
        public abstract void StartPoll();
    }
}