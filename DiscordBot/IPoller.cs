using Discord.WebSocket;

namespace DiscordBot
{
    public interface IPoller
    {
        void StartPoll();
    }
}