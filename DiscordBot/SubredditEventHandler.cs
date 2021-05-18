using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using ClassLocator;
using Discord;
using Discord.WebSocket;

namespace DiscordBot
{
    public class SubredditEventHandler : IEventHandler
    {
        public Type Channel { get { return typeof(SubredditEvent); } }
        public OrderedDictionary SubmissionCache { get; set; }
        private const int _cacheMax = 20; // Reddit limits you to 30 API calls per minute

        public SubredditEventHandler()
        {
            SubmissionCache = new OrderedDictionary();
        }

        public async Task Fire(IEvent firedEvent)
        {
            var subEvent = firedEvent as SubredditEvent;
            var channel = GetChannel(subEvent.Server, subEvent.ServerChannel);

            if (!subEvent.IsRemoval)
            {
                // We have an event for a new found post
                var message = await channel.SendMessageAsync(subEvent.Submission.Message);
                SubmissionCache.Add(subEvent.Submission.Id, message);
            }
            else
            {
                // We have an event for a removed post
                var message = SubmissionCache[subEvent.Submission.Id] as IDeletable;
                await message.DeleteAsync();
            }

            TrimCache();
        }

        private void TrimCache()
        {
            while (SubmissionCache.Count > _cacheMax)
            {
                SubmissionCache.RemoveAt(0);
            }
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