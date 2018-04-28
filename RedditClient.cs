using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace discordBot
{
    public class RedditClient
    {
        private RestClient _client;
        private List<string> _lastNewSubmissionResult;

        private string _targetServer;
        private string _targetChannel;
        private string _targetSubreddit;
        private int _newSubmissionCacheSize;
        public string TargetChannel { get { return _targetChannel; } }
        public string TargetServer { get { return _targetServer; } }
        public string TargetSubreddit { get { return _targetSubreddit; } }

        public RedditClient(int cache, string targetServer, string targetChannel, string targetSubreddit)
        {
            _client = new RestClient(Constants.RedditURL);
            _targetChannel = targetChannel;
            _targetServer = targetServer;
            _targetSubreddit = targetSubreddit;
            _newSubmissionCacheSize = cache;

            _lastNewSubmissionResult = new List<string>();

            InitializeSubredditSubmissions();
        }

        private void InitializeSubredditSubmissions()
        {
            var jobj = FetchSubredditNewSubmissions();
            var tempStack = new Stack<string>();
            foreach (var submission in jobj["data"]["children"])
            {
                _lastNewSubmissionResult.Add(submission["data"]["name"].ToString());
            }
        }

        private JObject FetchSubredditNewSubmissions()
        {
            var req = new RestRequest("/r/" + _targetSubreddit + "/new.json");
            req.AddParameter("raw_json", 1);
            req.AddParameter("limit", _newSubmissionCacheSize);
            var res = _client.Execute(req);
            var jobj = JObject.Parse(res.Content);

            return jobj;
        }
        public IEnumerable<string> UpdateReddit()
        {
            var results = new List<string>();
            var _newLastNew = new List<string>();
            var jobj = FetchSubredditNewSubmissions();

            foreach (var submission in jobj["data"]["children"])
            {
                _newLastNew.Add(submission["data"]["name"].ToString());

                if (!_lastNewSubmissionResult.Contains(submission["data"]["name"].ToString()))
                {
                    results.Add(submission["data"]["title"].ToString() + ": " + submission["data"]["url"]);
                }
            }

            _lastNewSubmissionResult = _newLastNew;
            return results;
        }
    }
}


