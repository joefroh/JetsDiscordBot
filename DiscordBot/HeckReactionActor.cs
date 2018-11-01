using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace discordBot
{
    public class HeckReactionActor
    {
        public string TriggerString { get { return "hell"; } }

        public async Task React(SocketMessage msg)
        {
            var guild = ((SocketGuildChannel)msg.Channel).Guild;
            IEmote emote = guild.Emotes.First(e => e.Name == "heck");
            var message = (RestUserMessage)msg.Channel.GetMessageAsync(msg.Id).Result;
            await message.AddReactionAsync(emote);
        }
    }
}