using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace discordBot
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

        public RedditClient(SubredditConfig config)
        {
            _client = new RestClient(Constants.RedditURL);
            _subConfig = config;

            _lastNewSubmissionResult = new List<string>();

            InitializeSubredditSubmissions();
        }

        private void InitializeSubredditSubmissions()
        {
            var jobj = FetchSubredditNewSubmissions(1);
            _newestSubmissionTime = ulong.Parse(jobj["data"]["children"][0]["data"]["created_utc"].ToString());
        }

        private JObject FetchSubredditNewSubmissions(int limit)
        {
            var req = new RestRequest("/r/" + _subConfig.TargetSubreddit + "/new.json");
            req.AddParameter("raw_json", 1);
            req.AddParameter("limit", limit);
            req.AddParameter("sort", "new");

            var res = _client.Execute(req); // TODO check IsSuccessful and fail gracefully.
            var jobj = JObject.Parse(res.Content);

            return jobj;
        }

        public IEnumerable<string> UpdateReddit()
        {
            var results = new List<string>();
            ulong tempNewestTime = 0;

            var jobj = FetchSubredditNewSubmissions(_subConfig.NewSubmissionCacheSize);

            foreach (var submission in jobj["data"]["children"])
            {
                var submissionTime = ulong.Parse(submission["data"]["created_utc"].ToString());

                // if the submission is newer than the newest one we saw last poll
                if (submissionTime > _newestSubmissionTime)
                {
                    var builder = new StringBuilder();
                    builder.AppendLine(submission["data"]["title"].ToString() + ": " + submission["data"]["url"]);
                    builder.Append("Comments: " + Constants.RedditURL + submission["data"]["permalink"]);
                    results.Add(builder.ToString());
                }

                // if the current submission is newer, update the temp time
                if (tempNewestTime < submissionTime)
                {
                    tempNewestTime = submissionTime;
                }
            }

            _newestSubmissionTime = tempNewestTime;
            return results;
        }
    }
}


