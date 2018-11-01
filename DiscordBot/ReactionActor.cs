using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace discordBot
{
    public class ReactionActor
    {
        ReactionActorConfig _config;

        public ReactionActor(ReactionActorConfig config)
        {
            _config = config;
        }

        public string Trigger
        {
            get
            {
                return _config.Trigger;
            }
        }
        public async Task React(SocketMessage msg)
        {
            // check to see if the only match is an ignored term
            if (MessageTriggerOnOnlyIgnoredWords(msg.Content))
            {
                return;
            }

            var guild = ((SocketGuildChannel)msg.Channel).Guild;
            IEmote emote = guild.Emotes.FirstOrDefault(e => e.Name == _config.Reaction);
            var message = (RestUserMessage)msg.Channel.GetMessageAsync(msg.Id).Result;
            await message.AddReactionAsync(emote);
        }

        private bool MessageTriggerOnOnlyIgnoredWords(string message)
        {
            if (_config.Ignore == null)
                return false;
                
            var tokens = message.Split(' ');

            foreach (var token in tokens)
            {
                foreach (var ignoreWord in _config.Ignore)
                {
                    if (token.ToLower().Contains(Trigger) && !token.ToLower().Contains(ignoreWord))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}