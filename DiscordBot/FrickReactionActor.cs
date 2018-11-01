using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace discordBot
{
    public class FrickReactionActor
    {
        public string TriggerString { get { return "fuck"; } }

        public async Task React(SocketMessage msg)
        {
            var guild = ((SocketGuildChannel)msg.Channel).Guild;
            IEmote emote = guild.Emotes.First(e => e.Name == "frick");
            var message = (RestUserMessage)msg.Channel.GetMessageAsync(msg.Id).Result;
            await message.AddReactionAsync(emote);
        }
    }
}