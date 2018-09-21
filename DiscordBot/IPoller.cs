using Discord.WebSocket;

namespace discordBot
{
    public interface IPoller
    {
        void StartPoll();
    }
}