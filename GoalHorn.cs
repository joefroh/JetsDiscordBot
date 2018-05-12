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
        private GoalHornConfig _config;
        public GoalHorn(SocketTextChannel textChannel, GoalHornConfig config)
        {
            _restClient = new RestClient(Constants.NHLApiEndpoint);
            _channel = textChannel;
            _config = config;
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
                if (game != null)
                {
                    var gameFinal = IsGameFinal(game);

                    while (!gameFinal)
                    {
                        //If a game is found today, check if its already started
                        if (IsGameInProgress(game))
                        {
                            RunGame(game);
                            gameFinal = true;
                        }

                        if (!gameFinal)
                        {
                            AwaitGameStart(game);
                        }
                    }
                }

                //Sleep for 5 hours
                var span = TimeSpan.FromHours(5);
                Console.WriteLine("Game complete or no " + _config.TeamFriendlyName + " game today detected, sleeping for " + span.ToString());
                Thread.Sleep(span);
            }
        }

        private void RunGame(GameDetail game)
        {
            var gameLive = true;
            var lastGoalId = -1;
            while (gameLive)
            {
                var gameObj = GetLiveGameData(game);
                var currGoal = GetLatestGoal(gameObj);
                if (currGoal != null)
                {
                    var currGoalId = int.Parse(currGoal["about"]["eventId"].ToString());
                    var currGoalIndx = int.Parse(currGoal["about"]["eventIdx"].ToString());

                    if (currGoalId > lastGoalId)
                    {
                        //we have a new goal, check if its the team we care about
                        var goal = GetGoalFromIndex(gameObj, currGoalIndx);

                        if (goal.ScoringTeamId == _config.Team)
                        {
                            // if we want to post the goal, delay first to let the data populate
                            Thread.Sleep(_config.Delay * 1000);
                            gameObj = GetLiveGameData(game);
                            currGoal = GetLatestGoal(gameObj);
                            if (currGoal != null)
                            {
                                currGoalIndx = int.Parse(currGoal["about"]["eventIdx"].ToString());
                                goal = GetGoalFromIndex(gameObj, currGoalIndx);

                                //output to bot
                                AnnounceGoal(goal);
                            }
                        }
                        lastGoalId = currGoalId;
                    }
                }

                gameLive = IsGameInProgress(gameObj);
                Thread.Sleep(1000);
            }
        }

        private void AnnounceGoal(GoalDetail goal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(String.Format("{0} {1} GOOOOOAAAALLLL!!!!! {2}", _config.PreText, _config.TeamFriendlyName.ToUpper(), _config.PostText));
            builder.AppendLine(goal.Description);

            _channel.SendMessageAsync(builder.ToString());
        }

        private void AwaitGameStart(GameDetail game)
        {
            // calculate time to game and sleep, or if its after the scheduled time, just wait 5 seconds or something
            var gametime = game.StartTimeUTC;
            var spanTilGame = gametime.Subtract(DateTime.UtcNow);

            if (spanTilGame.Ticks > 0) // check to make sure there is still time before the scheduled puck drop
            {
                Console.WriteLine("Sleeping until " + _config.TeamFriendlyName + " game in " + spanTilGame.ToString());
                Thread.Sleep(spanTilGame);
                Console.WriteLine("Waking up! Is it gametime?");
            }
            else //otherwise just wait 5 seconds and check again
            {
                Thread.Sleep(5000);
            }
        }

        #region Game Manipulation Methods
        private GameDetail GetTodaysGame()
        {
            var req = new RestRequest("/api/v1/schedule");
            req.AddParameter("teamId", _config.Team);
            var res = _restClient.Execute(req);
            var resObj = JObject.Parse(res.Content);
            var dates = (JArray)resObj["dates"];

            if (dates.Count == 0)
            {
                return null;
            }

            var gameDates = (JArray)dates[0]["games"];

            if (gameDates.Count == 0)
            {
                return null;
            }

            // There should only ever be 1
            var gameDate = DateTime.Parse(gameDates[0]["gameDate"].ToString());
            var gameLiveLink = gameDates[0]["link"].ToString();

            var result = new GameDetail() { LiveFeedLink = gameLiveLink, StartTimeUTC = gameDate };
            return result;
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
        private JToken GetLatestGoal(JObject gameObj)
        {
            var scoringArr = (JArray)gameObj["liveData"]["plays"]["scoringPlays"];

            if (scoringArr.Count > 0)
            {
                var goalIndx = int.Parse(scoringArr.Last.ToString());
                var playArray = (JArray)gameObj["liveData"]["plays"]["allPlays"];

                return playArray[goalIndx];
            }

            return null;
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