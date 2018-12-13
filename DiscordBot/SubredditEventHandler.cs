using System;
using System.Threading.Tasks;
using ClassLocator;
using Discord;
using Discord.WebSocket;

namespace DiscordBot
{
    public class SubredditEventHandler : IEventHandler
    {
        public Type Channel { get { return typeof(SubredditEvent); } }

        public async Task Fire(IEvent firedEvent)
        {
            var subEvent = firedEvent as SubredditEvent;
            var channel = GetChannel(subEvent.Server, subEvent.ServerChannel);

            await channel.SendMessageAsync(subEvent.Submission);
        }

        private ITextChannel GetChannel(ulong server, ulong channel)
        {
            var discordClient = Locator.Instance.Fetch<IDiscordClient>();
            var guild = discordClient.GetGuildAsync(server).Result;

            if (null == guild)
                return null;

            var textChannel = guild.GetTextChannelAsync(channel).Result;

            if (null == textChannel)
                return null;

            return textChannel;
        }
    }
}