using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLocator;
using Discord.WebSocket;
using NHLApi;

namespace discordBot
{
    public class ScheduleCommandExecutor : ICommandExecutor
    {

        private readonly string helpText = "";

        public ScheduleCommandExecutor()
        {
            var builder = new StringBuilder();
            builder.AppendLine(Locator.Instance.Fetch<IConfigurationLoader>().Configuration.CommandPrefix + CommandString + " <function> <team>");

            builder.Append("Functions: ");
            foreach (var function in Enum.GetNames(typeof(ScheduleCommandEnum)))
            {
                builder.Append("`" + function + "` ");
            }

            builder.AppendLine();
            helpText = builder.ToString(); // HELP TEXT IS ASSIGNED HERE
        }

        public override string CommandString
        {
            get
            {
                return "sched";
            }
        }

        public override string HelpText
        {
            get
            {
                return helpText; // this should be help text on how to use the function
            }
        }

        public async override Task ExecuteCommand(SocketMessage msg)
        {
            var message = msg.Content.Split(' '); //TODO make a method that does this nicely, this will get repeated a lot (the following few lines)

            if (message.Length < 3)
            {
                await CommandExecutorHelpers.ErrorMessage(msg);
                return;
            }

            var team = string.Join(" ", message.Skip(2));
            var commandString = message[1];
            ScheduleCommandEnum command = 0;

            // Generate an enum from a string, enforces the command exists
            try
            {
                command = (ScheduleCommandEnum)Enum.Parse(typeof(ScheduleCommandEnum), commandString, true); // ignore case
            }
            catch (ArgumentException)
            {
                await CommandExecutorHelpers.ErrorMessage(msg, String.Format("I'm sorry, I didn't recognize the command: {0}", commandString));
                return;
            }

            var result = GenerateScheduleResult(team, command);

            if (result.Count() == 0)
            {
                await CommandExecutorHelpers.ErrorMessage(msg);
                return;
            }

            foreach (var response in result)
            {
                await msg.Channel.SendMessageAsync(response);
            }
        }

        private IEnumerable<string> GenerateScheduleResult(string team, ScheduleCommandEnum command)
        {
            switch (command)
            {
                case ScheduleCommandEnum.NextGame:
                    return NextGame(team);
                case ScheduleCommandEnum.LastGame:
                    return LastGame(team);
                case ScheduleCommandEnum.LastGameHighlights:
                    return LastGameHighlights(team);
                case ScheduleCommandEnum.LastGameLineScore:
                    return LastGameLineScore(team);
                case ScheduleCommandEnum.Lookup:
                    return Lookup(team);
                default:
                    return new List<string>(); //TODO revisit this decision. Empty string puts error checking honous above.
            }
        }

        private IEnumerable<string> Lookup(string team)
        {
            var result = new List<string>();
            var nameMappings = Locator.Instance.Fetch<TeamNameTranslator>().LookupIdsForName(team);
            if (nameMappings.Count == 0)
            {
                result.Add("Couldn't find team: " + team);
                return result;
            }

            if (nameMappings.Count > 1)
            {
                result.Add("Multiple results found for that team, which did you mean?");
                result.Add(String.Format("{0} or {1}?", nameMappings[0].Value.First(), nameMappings[1].Value.First()));
                return result;
            }

            result.Add(nameMappings.First().Value.First() + " " + nameMappings.First().Key);
            return result;
        }

        private IEnumerable<string> LastGameLineScore(string team)
        {
             List<string> result = new List<string>();
             int teamId = -1;

            var isTeamId = int.TryParse(team, out teamId);
            if (!isTeamId)
            {
                var Ids = Locator.Instance.Fetch<TeamNameTranslator>().LookupIdsForName(team);
                if (Ids.Count > 1)
                {
                    result.Add("Name conflict, got more than one team ID for " + team + ". Please be more specific.");
                    return result;
                }

                if (Ids.Count == 0)
                {
                    result.Add("Couldn't find a team by the name " + team + ". Please try another name.");
                    return result;
                }

                teamId = Ids.First().Key;
            }
           

            var api = new NHLApiClient();
            var lastGame = api.GetLastGame(teamId);
            var teamData = api.GetTeam(teamId);

            if (lastGame == null || lastGame.TotalGames < 1)
            {
                result.Add(string.Format("There are currently no previous games the {0}", teamData.Name));
                return result;
            }

            var homeTeam = api.GetTeam(lastGame.Dates[0].Games[0].Teams.Home.Team.Id);
            var awayTeam = api.GetTeam(lastGame.Dates[0].Games[0].Teams.Away.Team.Id);
            var gameData = api.GetLiveGameDetail(lastGame.Dates[0].Games[0].GamePk);
            var lineScore = api.GetGameLineScore(lastGame.Dates[0].Games[0].GamePk);

            foreach (var period in lineScore.Periods)
            {
                var summary = NHLInformationSummarizer.PeriodSummary(period, homeTeam, awayTeam, gameData);
                result.Add(summary.ToString());
            }
            return result;
        }

        private IEnumerable<string> NextGame(string team)
        {
            List<string> result = new List<string>();
            NHLApiClient api = new NHLApiClient();
            int teamId = -1;

            var isTeamId = int.TryParse(team, out teamId);
            if (!isTeamId)
            {
                var Ids = Locator.Instance.Fetch<TeamNameTranslator>().LookupIdsForName(team);
                if (Ids.Count > 1)
                {
                    result.Add("Name conflict, got more than one team ID for " + team + ". Please be more specific.");
                    return result;
                }

                if (Ids.Count == 0)
                {
                    result.Add("Couldn't find a team by the name " + team + ". Please try another name.");
                    return result;
                }

                teamId = Ids.First().Key;
            }


            var nextGame = api.GetNextGame(teamId);
            var teamData = api.GetTeam(teamId);

            if (nextGame.TotalGames < 1)
            {
                result.Add(string.Format("There are currently no games scheduled for the {0}", teamData.Name));
                return result;
            }

            result.Add(NHLInformationSummarizer.NextGameSummary(nextGame, teamData));
            return result;
        }

        private IEnumerable<string> LastGame(string team)
        {
            List<string> result = new List<string>();
            NHLApiClient api = new NHLApiClient();
            int teamId = -1;

            var isTeamId = int.TryParse(team, out teamId);
            if (!isTeamId)
            {
                var Ids = Locator.Instance.Fetch<TeamNameTranslator>().LookupIdsForName(team);
                if (Ids.Count > 1)
                {
                    result.Add("Name conflict, got more than one team ID for " + team + ". Please be more specific.");
                    return result;
                }

                if (Ids.Count == 0)
                {
                    result.Add("Couldn't find a team by the name " + team + ". Please try another name.");
                    return result;
                }

                teamId = Ids.First().Key;
            }
            

            var lastGame = api.GetLastGame(teamId);
            var teamData = api.GetTeam(teamId);

            if (lastGame == null || lastGame.TotalGames < 1)
            {
                result.Add(string.Format("There are currently no previous games the {0}", teamData.Name));
                return result;
            }

            var game = lastGame.Dates[0].Games[0];
            var homeTeam = api.GetTeam(game.Teams.Home.Team.Id);
            var awayTeam = api.GetTeam(game.Teams.Away.Team.Id);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine(string.Format("The last {0} game was {1}, {2} @ {3} at {4}", teamData.Name, game.GameDate.ToLocalTime().ToLongDateString(), game.Teams.Away.Team.Name, game.Teams.Home.Team.Name, game.Venue.Name));
            builder.AppendLine(string.Format("The final score was {0} {1} : {2} {3}", awayTeam.Abbreviation, game.Teams.Away.Score, homeTeam.Abbreviation, game.Teams.Home.Score));
            result.Add(builder.ToString());

            return result;
        }

        private IEnumerable<string> LastGameHighlights(string team)
        {
            List<string> result = new List<string>();
            NHLApiClient api = new NHLApiClient();
            int teamId = -1;

            var isTeamId = int.TryParse(team, out teamId);
            if (!isTeamId)
            {
                var Ids = Locator.Instance.Fetch<TeamNameTranslator>().LookupIdsForName(team);
                if (Ids.Count > 1)
                {
                    result.Add("Name conflict, got more than one team ID for " + team + ". Please be more specific.");
                    return result;
                }

                if (Ids.Count == 0)
                {
                    result.Add("Couldn't find a team by the name " + team + ". Please try another name.");
                    return result;
                }

                teamId = Ids.First().Key;
            }
            
            var lastGame = api.GetLastGame(teamId);
            var gameId = lastGame.Dates[0].Games[0].GamePk;

            var content = api.GetGameContent(gameId);

            foreach (var highlight in content.Highlights.Scoreboard.Items)
            {
                if (highlight.Type != "video")
                    continue;

                var builder = new StringBuilder();
                var vid = highlight.Playbacks.Last();
                builder.AppendLine(highlight.Title);
                builder.AppendLine(vid.URL);

                result.Add(builder.ToString());
            }

            return result;
        }
    }
}