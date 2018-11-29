using Discord.WebSocket;

namespace discordBot{
    interface IMessageLogger
    {
        void LogMessage(SocketMessage message);
    }
}