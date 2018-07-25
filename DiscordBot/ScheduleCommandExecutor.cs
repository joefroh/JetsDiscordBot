using System;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using NHLApi;

namespace discordBot
{
    public class ScheduleCommandExecutor : ICommandExecutor
    {
        public ScheduleCommandExecutor(Configuration config) : base(config)
        {

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
                return "sched <teamname> next"; // this should be help text on how to use the function
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

            if (result == "")
            {
                await CommandExecutorHelpers.ErrorMessage(msg);
                return;
            }

            await msg.Channel.SendMessageAsync(result);
        }

        private string GenerateScheduleResult(string team, ScheduleCommandEnum command)
        {
            switch (command)
            {
                case ScheduleCommandEnum.NextGame:
                    return NextGame(team);
                case ScheduleCommandEnum.LastGame:
                    return LastGame(team);
                default:
                    return ""; //TODO revisit this decision. Empty string puts error checking honous above.
            }
        }

        private string NextGame(string team)
        {
            NHLApiClient api = new NHLApiClient();
            var nextGame = api.GetNextGame(int.Parse(team));
            var teamData = api.GetTeam(int.Parse(team));

            if (nextGame.TotalGames < 1)
            {
                return string.Format("There are currently no games scheduled for the {0}", teamData.Name);
            }

            var game = nextGame.Dates[0].Games[0];
            return string.Format("The next {0} game is {1}, {2} @ {3} at {4}", teamData.Name, game.GameDate.ToLocalTime().ToLongDateString(), game.Teams.Away.Team.Name, game.Teams.Home.Team.Name, game.Venue.Name);
        }

        private string LastGame(string team)
        {
            NHLApiClient api = new NHLApiClient();
            var teamId = int.Parse(team);
            var lastGame = api.GetLastGame(teamId);
            var teamData = api.GetTeam(teamId);

            if (lastGame.TotalGames < 1)
            {
                return string.Format("There are currently no previous games the {0}", teamData.Name);
            }

            var game = lastGame.Dates[0].Games[0];
            var homeTeam = api.GetTeam(game.Teams.Home.Team.Id);
            var awayTeam = api.GetTeam(game.Teams.Away.Team.Id);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine(string.Format("The last {0} game was {1}, {2} @ {3} at {4}", teamData.Name, game.GameDate.ToLocalTime().ToLongDateString(), game.Teams.Away.Team.Name, game.Teams.Home.Team.Name, game.Venue.Name));
            builder.AppendLine(string.Format("The final score was {0} {1} : {2} {3}", awayTeam.Abbreviation, game.Teams.Away.Score, homeTeam.Abbreviation, game.Teams.Home.Score));
            return builder.ToString();
        }
    }
}