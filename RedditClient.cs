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
        private Configuration _config;
        private Queue<string> _recentPosts;
        public RedditClient(Configuration config)
        {
            _client = new RestClient("https://www.reddit.com");
            _config = config;
            _recentPosts = new Queue<string>();

            InitializeSubredditPosts();
        }

        private void InitializeSubredditPosts()
        {
            var jobj = FetchSubredditNewSubmissions();
            var tempStack = new Stack<string>();
            foreach (var submission in jobj["data"]["children"])
            {
                tempStack.Push(submission["data"]["name"].ToString());
            }

            while (tempStack.Count != 0)
            {
                _recentPosts.Enqueue(tempStack.Pop());
            }
        }

        private JObject FetchSubredditNewSubmissions()
        {
            var req = new RestRequest("/r/" + _config["TargetSubreddit"] + "/new.json");
            req.AddParameter("raw_json", 1);
            req.AddParameter("limit", 25);
            var res = _client.Execute(req);
            var jobj = JObject.Parse(res.Content);

            return jobj;
        }
        public IEnumerable<string> UpdateReddit()
        {
            var results = new List<string>();
            var jobj = FetchSubredditNewSubmissions();

            foreach (var submission in jobj["data"]["children"])
            {
                if (!_recentPosts.Contains(submission["data"]["name"].ToString()))
                {
                    results.Add(submission["data"]["title"].ToString() + ": " + submission["data"]["url"]);
                    if (_recentPosts.Count >= 25)
                        _recentPosts.Dequeue();
                    _recentPosts.Enqueue(submission["data"]["name"].ToString());
                }
            }

            return results;
        }
    }
}


