using System.Threading.Tasks;
using Discord.WebSocket;

namespace discordBot
{
    public class CommandHandler
    {
        private Configuration _config;
        public CommandHandler(Configuration config)
        {
            _config = config;
        }

        public async Task HandleCommand(SocketMessage message)
        {
            var command = message.Content.Split(' ');
            if (command[0].StartsWith(_config["CommandPrefix"]))
            {
                switch (command[0].ToLower().Split(_config["CommandPrefix"])[1]) // lop off the command prefix 
                {
                    case "ping":
                        await message.Channel.SendMessageAsync("Pong.");
                        break;
                    case "echo":
                        await message.Channel.SendMessageAsync(message.Content.Remove(0, 4 + _config["CommandPrefix"].Length));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}