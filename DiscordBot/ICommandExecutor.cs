using System.Threading.Tasks;
using Discord.WebSocket;

namespace discordBot
{
    public abstract class ICommandExecutor
    {
        public ICommandExecutor() { }
        public abstract Task ExecuteCommand(SocketMessage msg);
        public abstract string CommandString { get; }
        public abstract string HelpText { get; }
    }
}