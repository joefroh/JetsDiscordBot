using System;
using System.Text;
using System.Threading;
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
            _restClient = new RestClient(Constants.NHLApiEndpoint);
            _channel = textChannel;
            _teamID = teamId;
        }

        public void Run()
        {
            Task.Run(() => Poll());
        }

        //TODO: Clean up this ungodly mess of a method. There has got to be another way.
        private void Poll()
        {
            while (true)
            {
                //Check for the next game once a day
                var game = GetTodaysGame();
                if (game != null)
                {
                    var gameFinal = IsGameFinal(game);

                    while (!gameFinal)
                    {
                        //If a game is found today, check if its already started
                        if (IsGameInProgress(game))
                        {
                            var gameLive = true;
                            var lastGoalIdx = -1;
                            while (gameLive)
                            {
                                var gameObj = GetLiveGameData(game);
                                var currGoalIdx = GetLatestGoalIndex(gameObj);

                                if (currGoalIdx > lastGoalIdx)
                                {
                                    //we have a new goal, check if its the team we care about
                                    var goal = GetGoalFromIndex(gameObj, currGoalIdx);

                                    if (goal.ScoringTeamId == _teamID)
                                    {
                                        //output to bot
                                        StringBuilder builder = new StringBuilder();
                                        builder.AppendLine("GOOOOOAAAAAALLLLL!!!!");
                                        builder.AppendLine(goal.Description);
                                        builder.AppendLine(@"https://media.giphy.com/media/xTiQyxssUcbkVRE2v6/giphy.gif");
                                    }
                                    lastGoalIdx = currGoalIdx;
                                }

                                gameLive = IsGameInProgress(gameObj);
                                Thread.Sleep(1000);
                            }
                            gameFinal = true;
                        }

                        if (!gameFinal)
                        {
                            // calculate time to game and sleep, or if its after the scheduled time, just wait 5 seconds or something
                            var now = DateTime.UtcNow;
                            var gametime = game.StartTimeUTC;

                            var span = gametime.Subtract(now);

                            if (span.Ticks > 0) // check to make sure there is still time before the scheduled puck drop
                            {
                                Console.WriteLine("Sleeping until game time in " + span.ToString());
                                Thread.Sleep(span);
                            }
                            else //otherwise just wait 5 seconds and check again
                            {
                                Thread.Sleep(5000);
                            }
                        }
                    }
                }
            }
        }

        #region Game Manipulation Methods
        private GameDetail GetTodaysGame()
        {
            var req = new RestRequest("/api/v1/schedule");
            req.AddParameter("teamId", _teamID);
            var res = _restClient.Execute(req);
            var resObj = JObject.Parse(res.Content);
            var dates = (JArray)resObj["dates"][0]["games"];

            if (dates.Count > 0)
            {
                // There should only ever be 1
                var gameDate = DateTime.Parse(dates[0]["gameDate"].ToString());
                var gameLiveLink = dates[0]["link"].ToString();

                var result = new GameDetail() { LiveFeedLink = gameLiveLink, StartTimeUTC = gameDate };
                return result;
            }

            return null;
        }

        private bool IsGameInProgress(GameDetail game)
        {
            var stateString = GetAbstractGameState(game);

            if (stateString != "Live")
            {
                return false;
            }

            return true;
        }

        private bool IsGameInProgress(JObject gameObj)
        {
            var stateString = GetAbstractGameState(gameObj);

            if (stateString != "Live")
            {
                return false;
            }

            return true;
        }

        private bool IsGameFinal(GameDetail game)
        {
            var stateString = GetAbstractGameState(game);

            if (stateString != "Final")
            {
                return false;
            }

            return true;
        }

        private bool IsGameFinal(JObject gameObj)
        {
            var stateString = GetAbstractGameState(gameObj);

            if (stateString != "Final")
            {
                return false;
            }

            return true;
        }

        private string GetAbstractGameState(GameDetail game)
        {
            var gameObj = GetLiveGameData(game);
            return GetAbstractGameState(gameObj);
        }

        private string GetAbstractGameState(JObject gameObj)
        {
            var stateString = gameObj["gameData"]["status"]["abstractGameState"].ToString();

            return stateString;
        }

        private JObject GetLiveGameData(GameDetail game)
        {
            var req = new RestRequest(game.LiveFeedLink);
            var res = _restClient.Execute(req);
            var gameObj = JObject.Parse(res.Content);

            return gameObj;
        }
        private int GetLatestGoalIndex(JObject gameObj)
        {
            var currGoal = -1;
            var scoringArr = (JArray)gameObj["liveData"]["plays"]["scoringPlays"];

            if (scoringArr.Count > 0)
            {
                currGoal = int.Parse(scoringArr.Last.ToString());
            }

            return currGoal;
        }

        private GoalDetail GetGoalFromIndex(JObject gameObj, int index)
        {
            var allPlays = (JArray)gameObj["liveData"]["plays"]["allPlays"];
            var goalData = allPlays[index];

            var description = goalData["result"]["description"].ToString();
            var playIndex = index;
            var teamName = goalData["team"]["name"].ToString();
            var teamId = int.Parse(goalData["team"]["id"].ToString());

            var goalDetail = new GoalDetail() { Description = description, PlayIndex = playIndex, ScoringTeam = teamName, ScoringTeamId = teamId };

            return goalDetail;
        }
        #endregion
    }
}