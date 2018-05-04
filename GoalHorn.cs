using System;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace discordBot
{
    class GoalHorn
    {
        private RestClient _client;
        private SocketTextChannel _channel;

        public GoalHorn(SocketTextChannel textChannel)
        {
            _client = new RestClient("https://statsapi.web.nhl.com/api/v1/game/2017030234/feed/live");
            _channel = textChannel;
        }

        public GoalDetail GetLatestGoal()
        {
            var res = _client.Execute(new RestRequest());
            var jobj = JObject.Parse(res.Content.ToString());
            var currGoal = 0;
            GoalDetail result = null;

            if (((JArray)jobj["liveData"]["plays"]["scoringPlays"]).Count > 0)
            {
                currGoal = int.Parse(jobj["liveData"]["plays"]["scoringPlays"].Last.ToString());
            }

            if (currGoal > 0)
            {
                var playArray = (JArray)jobj["liveData"]["plays"]["allPlays"];
                var goalPlay = playArray[currGoal];
                var goalDesc = goalPlay["result"]["description"].ToString();
                var goalTeam = goalPlay["team"]["name"].ToString();

                result = new GoalDetail() { PlayIndex = currGoal, Description = goalDesc, ScoringTeam = goalTeam };
            }

            return result;
        }
    }
}