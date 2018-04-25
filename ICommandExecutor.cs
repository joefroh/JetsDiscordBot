using System.Threading.Tasks;
using Discord.WebSocket;

namespace discordBot
{
    public abstract class ICommandExecutor
    {
        protected Configuration _config;
        protected ICommandExecutor(Configuration config) => _config = config;
        public abstract Task ExecuteCommand(SocketMessage msg);
        public abstract string CommandString { get; }
        public abstract string HelpText { get; }
    }
}