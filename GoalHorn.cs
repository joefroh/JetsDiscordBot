using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace discordBot
{
    class GoalHorn
    {
        private RestClient _restClient;
        private SocketTextChannel _channel;
        private int _teamID;

        public GoalHorn(SocketTextChannel textChannel, int teamId)
        {
            //_client = new RestClient("https://statsapi.web.nhl.com/api/v1/game/2017030234/feed/live");
            _restClient = new RestClient(Constants.NHLApiEndpoint);
            _channel = textChannel;
            _teamID = teamId;
        }

        public void Run()
        {
            Task.Run(() => Poll());
        }

        private void Poll()
        {
            while (true)
            {
                //Check for the next game once a day
                var game = GetTodaysGame();
                if (game != DateTime.MinValue)
                {
                    //If a game is found today, check if its already started, capture the feed link and set the poll to start at game time
                    //at game time start poll for 500 ms GetLatestGoal
                }
            }
        }

        private DateTime GetTodaysGame()
        {
            var req = new RestRequest("/api/v1/schedule");
            req.AddParameter("teamId", _teamID);
            var res = _restClient.Execute(req);
            var resObj = JObject.Parse(res.Content);
            var dates = (JArray)resObj["dates"][0]["games"];

            if (dates.Count > 0)
            {
                // There should only ever be 1
                var gameDateString = dates[0]["gameDate"].ToString();
                return DateTime.Parse(gameDateString);
            }

            return DateTime.MinValue;
        }
        private GoalDetail GetLatestGoal()
        {
            var res = _restClient.Execute(new RestRequest());
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