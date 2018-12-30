using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading;
using ClassLocator;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace DiscordBot
{
    public class RedditClient
    {
        private RestClient _client;
        private List<string> _lastNewSubmissionResult;
        private ulong _newestSubmissionTime;
        private SubredditConfig _subConfig;
        public ulong TargetChannel { get { return _subConfig.TargetChannelID; } }
        public ulong TargetServer { get { return _subConfig.ServerID; } }
        public string TargetSubreddit { get { return _subConfig.TargetSubreddit; } }
        public List<SubredditSubmission> SubredditPostCache { get; set; }

        public RedditClient(SubredditConfig config)
        {
            _client = new RestClient(Constants.RedditURL);
            _subConfig = config;

            _lastNewSubmissionResult = new List<string>();
            SubredditPostCache = new List<SubredditSubmission>();

            InitializeSubredditSubmissions();
        }

        private void InitializeSubredditSubmissions()
        {
            Locator.Instance.Fetch<ILogger>().LogLine("Initializing subreddit poller for /r/" + _subConfig.TargetSubreddit);
            var jobj = FetchSubredditNewSubmissions(1);
            _newestSubmissionTime = ulong.Parse(jobj["data"]["children"][0]["data"]["created_utc"].ToString());
        }

        private JObject FetchSubredditNewSubmissions(int limit)
        {
            var req = new RestRequest("/r/" + _subConfig.TargetSubreddit + "/new.json");
            req.AddParameter("raw_json", 1);
            req.AddParameter("limit", limit);
            req.AddParameter("sort", "new");

            Locator.Instance.Fetch<ILogger>().LogLine("Fetching newest posts in subreddit: /r/" + _subConfig.TargetSubreddit);
            var res = _client.Execute(req);

            if (!res.IsSuccessful)
            {
                // Fixes sporatic poller crash, if Reddit gives a bad response (busy or down)
                // Wait 10 minutes and recurse.

                Locator.Instance.Fetch<ILogger>().LogLine("Subreddit Poll failed for some reason. Waiting 10 mins and trying again.");
                Thread.Sleep(1000 * 60 * 10);
                Locator.Instance.Fetch<ILogger>().LogLine("Subreddit Poll is trying again after failure.");
                return FetchSubredditNewSubmissions(limit);
            }

            var jobj = JObject.Parse(res.Content);
            return jobj;
        }

        public SubredditUpdate UpdateReddit()
        {
            var jobj = FetchSubredditNewSubmissions(_subConfig.NewSubmissionCacheSize);
            var newSubmissions = GetNewestSubmissions(jobj);
            var removed = CheckCacheForRemoved();

            // Housekeeping
            LimitCache();

            //create return
            var result = new SubredditUpdate();
            result.NewSubmissions = newSubmissions;
            result.RemovedSubmissions = removed;

            return result;
        }

        private IEnumerable<SubredditSubmission> CheckCacheForRemoved()
        {
            List<SubredditSubmission> removalList = new List<SubredditSubmission>();

            foreach (var post in SubredditPostCache)
            {
                // Fetch the post data
                var req = new RestRequest(post.PermaLink + ".json");
                var response = _client.Execute(req);

                // If the request fails for some reason, just skip this for now.
                if (!response.IsSuccessful)
                {
                    continue;
                }
                var jobj = JArray.Parse(response.Content);
                var postInfo = jobj[0]["data"]["children"][0]["data"];

                // check if it is marked as removed
                if (postInfo["selftext"] != null && (postInfo["selftext"].ToString() == "[removed]" || postInfo["selftext"].ToString() == "[deleted]"))
                {
                    // if so, add its id to the list
                    removalList.Add(post);
                }
            }

            // remove posts from the cache, I would do this in line but thats a bad idea.
            foreach (var post in removalList)
            {
                SubredditPostCache.Remove(post);
            }

            return removalList;
        }

        private IEnumerable<SubredditSubmission> GetNewestSubmissions(JObject jobj)
        {
            ulong tempNewestTime = 0;
            var newSubmissions = new List<SubredditSubmission>();

            foreach (var submission in jobj["data"]["children"])
            {
                var submissionTime = ulong.Parse(submission["data"]["created_utc"].ToString());

                // if the submission is newer than the newest one we saw last poll
                if (submissionTime > _newestSubmissionTime)
                {
                    Locator.Instance.Fetch<ILogger>().LogLine("Found a new post in /r/" + _subConfig.TargetSubreddit);

                    //Add it to the cache first
                    var cacheitem = new SubredditSubmission();
                    cacheitem.Id = submission["data"]["id"].ToString();
                    cacheitem.PermaLink = submission["data"]["permalink"].ToString();
                    SubredditPostCache.Add(cacheitem);

                    // Then create the message
                    var builder = new StringBuilder();
                    builder.AppendLine(submission["data"]["title"].ToString() + ": " + submission["data"]["url"]);
                    builder.Append("Comments: " + Constants.RedditURL + submission["data"]["permalink"]);
                    cacheitem.Message = builder.ToString();

                    // Add it to the results output
                    newSubmissions.Add(cacheitem);
                }

                // if the current submission is newer, update the temp time
                if (tempNewestTime < submissionTime)
                {
                    tempNewestTime = submissionTime;
                }
            }

            _newestSubmissionTime = tempNewestTime;

            return newSubmissions;
        }

        private void LimitCache()
        {
            while (SubredditPostCache.Count > _subConfig.NewSubmissionCacheSize)
            {
                // remove the oldest post from the cache
                SubredditPostCache.RemoveAt(0);
            }
        }
    }
}


