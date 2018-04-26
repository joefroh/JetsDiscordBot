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
        public RedditClient(Configuration config)
        {
            _client = new RestClient("https://www.reddit.com");
            _config = config;
        }

        public IEnumerable<string> UpdateReddit()
        {
            var results = new List<string>();
            var req = new RestRequest("/r/winnipegjets/new.json");
            req.AddParameter("raw_json", 1);
            req.AddParameter("limit", 5);

            var res = _client.Execute(req);
            var obj = JsonConvert.DeserializeObject<ExpandoObject>(res.Content);
            var jobj = JObject.Parse(res.Content);
            foreach (var submission in jobj["data"]["children"])
            {
                results.Add(submission["data"]["title"].ToString() + ": " + submission["data"]["url"]);
            }

            return results;
        }
    }
}


