using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace discordBot
{
    public class ReactionExecutor : ICommandExecutor
    {
        public override string CommandString
        {
            get
            {
                return "test";
            }
        }

        public override string HelpText => throw new System.NotImplementedException();

        public async override Task ExecuteCommand(SocketMessage msg)
        {
            var guild = ((SocketGuildChannel)msg.Channel).Guild;
            IEmote emote = guild.Emotes.First(e => e.Name == "frick");
            var message =  (RestUserMessage) msg.Channel.GetMessageAsync(msg.Id).Result;
            await message.AddReactionAsync(emote);
        }
    }
}