using System.Threading.Tasks;
using Discord.WebSocket;

namespace DiscordBot
{
    public class PingCommandExececutor : ICommandExecutor
    {
        public PingCommandExececutor()
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