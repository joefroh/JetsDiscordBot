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
            builder.AppendLine(Locator.Instance.Fetch<IConfigurationLoader>().Configuration.CommandPrefix + CommandString + " <team Id Number> <function>");

            builder.Append("Functions: ");
            foreach (var function in Enum.GetNames(typeof(ScheduleCommandEnum)))
            {
                builder.Append("`" + function + "` ");
            }

            builder.AppendLine();
            builder.Append("Team name mapping is a work in progress, please stay tuned. The Jets are 52 btw.");
            helpText = builder.ToString();
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

            var team = message[1];
            var commandString = message[2];
            ScheduleCommandEnum command = 0;

            try
            {
                command = (ScheduleCommandEnum)Enum.Parse(typeof(ScheduleCommandEnum), commandString, true); // ignore case
            }
            catch (ArgumentException)
            {
                await CommandExecutorHelpers.ErrorMessage(msg, String.Format("I'm sorry, I didn't recognize the command: {0}", commandString));
                return;
            }

            // make sure the team id is a number until I sort out a better parsing solution.
            int teamId = -1;
            if (!int.TryParse(team, out teamId))
            {
                await CommandExecutorHelpers.ErrorMessage(msg);
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
            int teamId = int.Parse(team);
            switch (command)
            {
                case ScheduleCommandEnum.NextGame:
                    return NextGame(teamId);
                case ScheduleCommandEnum.LastGame:
                    return LastGame(teamId);
                case ScheduleCommandEnum.LastGameHighlights:
                    return LastGameHighlights(teamId);
                default:
                    return new List<string>(); //TODO revisit this decision. Empty string puts error checking honous above.
            }
        }

        private IEnumerable<string> NextGame(int team)
        {
            List<string> result = new List<string>();
            NHLApiClient api = new NHLApiClient();
            var nextGame = api.GetNextGame(team);
            var teamData = api.GetTeam(team);

            if (nextGame.TotalGames < 1)
            {
                result.Add(string.Format("There are currently no games scheduled for the {0}", teamData.Name));
                return result;
            }

            var game = nextGame.Dates[0].Games[0];
            result.Add(string.Format("The next {0} game is {1}, {2} @ {3} at {4}", teamData.Name, game.GameDate.ToLocalTime().ToLongDateString(), game.Teams.Away.Team.Name, game.Teams.Home.Team.Name, game.Venue.Name));
            return result;
        }

        private IEnumerable<string> LastGame(int team)
        {
            List<string> result = new List<string>();
            NHLApiClient api = new NHLApiClient();

            var lastGame = api.GetLastGame(team);
            var teamData = api.GetTeam(team);

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

        private IEnumerable<string> LastGameHighlights(int team)
        {
            List<string> result = new List<string>();
            NHLApiClient api = new NHLApiClient();
            var lastGame = api.GetLastGame(team);
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