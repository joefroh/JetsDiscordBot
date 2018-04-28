using System.Threading.Tasks;
using Discord.WebSocket;

namespace discordBot
{
    public class EchoCommandExececutor : ICommandExecutor
    {
        public EchoCommandExececutor(Configuration config) : base(config)
        {

        }
        public override string CommandString
        {
            get
            {
                return "echo";
            }
        }

        public override string HelpText => throw new System.NotImplementedException();

        public override async Task ExecuteCommand(SocketMessage msg)
        {
            var message = msg.Content.Remove(0, (this.CommandString.Length + _config.CommandPrefix.Length));
            await msg.Channel.SendMessageAsync(message);
        }
    }
}