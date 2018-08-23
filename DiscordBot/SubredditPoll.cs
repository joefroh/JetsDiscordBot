using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassLocator;
using Discord.WebSocket;

namespace discordBot
{
    class SubredditPoll : IPoller
    {
        private List<RedditClient> _subreddits;
        public SubredditPoll()
        {
            _subreddits = new List<RedditClient>();
            
            foreach (var subreddit in Locator.Instance.Fetch<IConfigurationLoader>().Configuration.SubredditConfig)
            {
                _subreddits.Add(new RedditClient(subreddit)); // TODO Walk this call tree and figure out how to get the "fail wait" off the main thread for init, only happens then.
            }
        }

        public void StartPoll()
        {
            Task.Run(() => Poll(Locator.Instance.Fetch<IConfigurationLoader>().Configuration.RedditRefreshTimer));
        }

        private void Poll(int pollRate)
        {
            while (true)
            {
                foreach (var subreddit in _subreddits)
                {
                    var guild = Locator.Instance.Fetch<DiscordSocketClient>().GetGuild(subreddit.TargetServer);
                    if (null == guild) continue;

                    var channel = guild.GetTextChannel(subreddit.TargetChannel);
                    if (null == channel) continue;

                    var newSubmissions = subreddit.UpdateReddit();
                    foreach (var sub in newSubmissions)
                    {
                        channel.SendMessageAsync(sub);
                    }
                }
                Thread.Sleep(pollRate * 60000/*minutes*/);
            }
        }
    }
}
