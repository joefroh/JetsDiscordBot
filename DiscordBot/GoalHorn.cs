using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClassLocator;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using NHLApi;
using RestSharp;

namespace DiscordBot
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
            Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: Running Goal Horn poller for {0}", _config.TeamFriendlyName));
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
                            Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: Game in progress for {0}", _config.TeamFriendlyName));
                            Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: Starting game listener for {0}", _config.TeamFriendlyName));
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
                Locator.Instance.Fetch<ILogger>().LogLine("Goal Horn: Game complete or no " + _config.TeamFriendlyName + " game today detected, sleeping for " + span.ToString());
                Thread.Sleep(span);
            }
        }

        private void RunGame(GameDetail game)
        {
            var gameLive = true;
            var lastGoalId = -1;
            Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: Enterting GameLive loop for {0}", _config.TeamFriendlyName));
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
                        Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: Detected goal in tracked game for {0}", _config.TeamFriendlyName));

                        //we have a new goal
                        var goal = GetGoalFromIndex(gameObj, currGoalIndx);
                        GoalDetail goalDetails = null;


                        // if we want to post the goal, delay first to let the data populate
                        Thread.Sleep(_config.Delay * 1000);
                        gameObj = GetLiveGameData(game);
                        goalDetails = GetGoalFromID(gameObj, currGoalId); // Fixes bug where if 2 goals are scored in a short period, we can announce the wrong goal.


                        if (goalDetails != null)
                        {
                            Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: Announcing goal in tracked game for {0}", _config.TeamFriendlyName));
                            if (goal.ScoringTeamId == _config.Team)
                            {
                                //output to bot
                                AnnouncePriorityGoal(goalDetails);
                            }
                            else
                            {
                                AnnounceGoal(goalDetails);
                            }
                        }
                        Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: Updating goal index to {1} for {0}", _config.TeamFriendlyName, currGoalId));
                        lastGoalId = currGoalId;
                    }
                }

                AwaitIntermission(game, gameObj);

                gameLive = IsGameInProgress(gameObj);
                Thread.Sleep(2000); // This used to be 1 second, but that seemed really fast.
            }

            Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: Exited game loop, running end game commands for {0}", _config.TeamFriendlyName));
            var gameData = GetLiveGameData(game);
            SendLineScoreForPeriod(gameData);
            SendGameSummary(gameData);
        }


        private void SendGameSummary(JObject gameObj)
        {
            List<string> result = new List<string>();

            var api = new NHLApiClient();

            var teamId = _config.Team;
            var scheduleData = api.GetTodaysSchedule(teamId);
            var nextGame = api.GetNextGame(teamId);
            var teamData = api.GetTeam(teamId);

            _channel.SendMessageAsync(NHLInformationSummarizer.GameSummary(scheduleData, nextGame, teamData));
        }

        private void AwaitIntermission(GameDetail game, JObject gameObj)
        {
            if (!GameIsInIntermission(gameObj))
                return;

            Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: Intermission detected in game for {0}", _config.TeamFriendlyName));

            // Send Period LineScore here
            SendLineScoreForPeriod(gameObj);

            while (GameIsInIntermission(gameObj))
            {
                Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: Checking if intermission is over for {0}", _config.TeamFriendlyName));
                Thread.Sleep(30000); // check every 30 seconds to see if intermission is over
                gameObj = GetLiveGameData(game);
            }
        }

        private void SendLineScoreForPeriod(JObject gameObj)
        {
            List<string> result = new List<string>();

            var api = new NHLApiClient();

            var homeTeam = api.GetTeam(int.Parse(gameObj["gameData"]["teams"]["home"]["id"].ToString()));
            var awayTeam = api.GetTeam(int.Parse(gameObj["gameData"]["teams"]["away"]["id"].ToString()));
            var gameData = api.GetLiveGameDetail(int.Parse(gameObj["gamePk"].ToString()));
            var lineScore = api.GetGameLineScore(int.Parse(gameObj["gamePk"].ToString()));

            Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: Sending period line score for {0}", _config.TeamFriendlyName));
            _channel.SendMessageAsync(NHLInformationSummarizer.PeriodSummary(lineScore.Periods.Last(), homeTeam, awayTeam, gameData));
        }

        private bool GameIsInIntermission(JObject gameObj)
        {
            return bool.Parse(gameObj["liveData"]["linescore"]["intermissionInfo"]["inIntermission"].ToString());
        }

        private void AnnouncePriorityGoal(GoalDetail goal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(String.Format("{0} {1} GOOOAAALLL!!! {2}", _config.PreText, _config.TeamFriendlyName.ToUpper(), _config.PostText));
            builder.AppendLine(goal.Description);

            _channel.SendMessageAsync(builder.ToString());
        }

        private void AnnounceGoal(GoalDetail goal)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(String.Format("{1} Goal", goal.ScoringTeam));
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
                Locator.Instance.Fetch<ILogger>().LogLine("Sleeping until " + _config.TeamFriendlyName + " game in " + spanTilGame.ToString());
                Thread.Sleep(spanTilGame);
                Locator.Instance.Fetch<ILogger>().LogLine("Waking up! Is it gametime?");
            }
            else //otherwise just wait 5 seconds and check again
            {
                Thread.Sleep(5000);
            }
        }

        #region Game Manipulation Methods
        private GameDetail GetTodaysGame()
        {
            Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: Fetching today's game schedule for {0}", _config.TeamFriendlyName));
            var req = new RestRequest("/api/v1/schedule");
            req.AddParameter("teamId", _config.Team);
            var res = _restClient.Execute(req);
            var resObj = JObject.Parse(res.Content);
            var dates = (JArray)resObj["dates"];

            if (dates.Count == 0)
            {
                Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: No dates found today for {0}", _config.TeamFriendlyName));
                return null;
            }

            var gameDates = (JArray)dates[0]["games"];

            if (gameDates.Count == 0)
            {
                Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: No games found today for {0}", _config.TeamFriendlyName));
                return null;
            }

            // There should only ever be 1
            Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Goal Horn: Found a game today for {0}", _config.TeamFriendlyName));
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

        private GoalDetail GetGoalFromID(JObject gameObj, int id)
        {
            var scoringPlays = (JArray)gameObj["liveData"]["plays"]["scoringPlays"];
            var allPlays = (JArray)gameObj["liveData"]["plays"]["allPlays"];

            foreach (var scoringPlay in scoringPlays)
            {
                var play = allPlays[int.Parse(scoringPlay.ToString())];
                if (int.Parse(play["about"]["eventId"].ToString()) == id)
                {
                    //Good Goal!
                    return GetGoalFromIndex(gameObj, int.Parse(scoringPlay.ToString()));
                }
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