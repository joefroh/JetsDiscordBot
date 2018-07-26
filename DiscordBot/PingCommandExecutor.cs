using System.Threading.Tasks;
using Discord.WebSocket;

namespace discordBot
{
    public class PingCommandExececutor : ICommandExecutor
    {
        public PingCommandExececutor(Configuration config) : base(config)
        {

        }
        public override string CommandString
        {
            get
            {
                return "ping";
            }
        }

        public override string HelpText
        {
            get
            {
                return "Checks if the bot is still up. It should say \"Pong\"";
            }
        }

        public override async Task ExecuteCommand(SocketMessage msg)
        {
            await msg.Channel.SendMessageAsync("Pong.");
        }
    }
}