using System.Threading.Tasks;
using Discord.WebSocket;

namespace discordBot
{
    public static class CommandExecutorHelpers
    {
        public async static Task ErrorMessage(SocketMessage msg)
        {
            await ErrorMessage(msg, "I'm sorry, I didn't understand what you were asking me to do. Please try again.");
        }

        public async static Task ErrorMessage(SocketMessage msg, string errorMessage)
        {
            await msg.Channel.SendMessageAsync(errorMessage);
        }
    }
}