using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassLocator;
using Discord.WebSocket;

namespace DiscordBot
{
    class SubredditPoll : IPoller
    {
        private List<RedditClient> _subreddits;
        public SubredditPoll()
        {
            _subreddits = new List<RedditClient>();
            var configs = Locator.Instance.Fetch<IConfigurationLoader>().Configuration.SubredditConfig;

            if (null == configs)
            {
                Locator.Instance.Fetch<ILogger>().LogLine("WARNING: NO SUBREDDIT CONFIGS FOUND IN CONFIGURATION FILE.");
                return;
            }

            foreach (var subreddit in configs)
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
                    var update = subreddit.UpdateReddit();

                    foreach (var submission in update.NewSubmissions)
                    {
                        var subEvent = new SubredditEvent(subreddit.TargetServer, subreddit.TargetChannel, submission);
                        Locator.Instance.Fetch<IEventBroker>().FireEvent(subEvent);
                    }

                    foreach (var removal in update.RemovedSubmissions)
                    {
                        var subRemoval = new SubredditEvent(subreddit.TargetServer, subreddit.TargetChannel, removal, true);
                        Locator.Instance.Fetch<IEventBroker>().FireEvent(subRemoval);
                    }
                }
                Thread.Sleep(pollRate * 60000/*minutes*/);
            }
        }
    }
}
