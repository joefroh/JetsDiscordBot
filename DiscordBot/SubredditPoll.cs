using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace discordBot
{
    class SubredditPoll : IPoller
    {
        private List<RedditClient> _subreddits;
        public SubredditPoll(DiscordSocketClient client, Configuration config) : base(client, config)
        {
            _subreddits = new List<RedditClient>();
            
            foreach (var subreddit in _config.SubredditConfig)
            {
                _subreddits.Add(new RedditClient(subreddit));
            }
        }

        public override void StartPoll()
        {
            Task.Run(() => Poll(_config.RedditRefreshTimer));
        }

        private void Poll(int pollRate)
        {
            while (true)
            {
                foreach (var subreddit in _subreddits)
                {
                    var guild = _client.GetGuild(subreddit.TargetServer);
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
