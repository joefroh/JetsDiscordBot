//THIS COMMAND HAS BEEN DISABLED INTENTIONALLY, UNCOMMENT TO USE AS A DEBUG IF YOU WANT.

using System.Threading.Tasks;
using ClassLocator;
using Discord.WebSocket;

namespace DiscordBot
{
    public class EchoCommandExececutor : ICommandExecutor
    {
        public EchoCommandExececutor()
        {

        }
        public override string CommandString
        {
            get
            {
                return "echo";
            }
        }

        public override string HelpText =>
            throw new System.NotImplementedException();

        public override async Task ExecuteCommand(SocketMessage msg)
        {
            var message = msg.Content.Remove(0, (this.CommandString.Length + Locator.Instance.Fetch<IConfigurationLoader>().Configuration.CommandPrefix.Length));
            await msg.Channel.SendMessageAsync(message);
        }
    }
}