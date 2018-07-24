using System;
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
            var message = msg.Content.Split(' ');

            if (message.Length < 3)
            {
                await ErrorMessage(msg);
            }

            var team = message[1];
            var command = message[2];

            // make sure the team id is a number until i sort out a better parsing solution.
            int teamId = -1;
            if (!int.TryParse(team, out teamId))
            {
                await ErrorMessage(msg);
            }

            var result = GenerateScheduleResult(team, command);
            await msg.Channel.SendMessageAsync(result);
        }

        private string GenerateScheduleResult(string team, string command)
        {
            NHLApiClient api = new NHLApiClient();
            var nextGame = api.GetNextGame(int.Parse(team));
            var teamData = api.GetTeam(int.Parse(team));

            if (nextGame.TotalGames < 1)
            {
                return string.Format("There are currently no games scheduled for the {0}", teamData.Name);
            }

            var game = nextGame.Dates[0].Games[0];
            return string.Format("The next {0} game is {1}, {2} @ {3} at {4}", teamData.Name, game.GameDate.ToLocalTime().ToLongDateString(), game.Teams.Home.Team.Name, game.Teams.Away.Team.Name, game.Venue.Name);
        }

        private async Task ErrorMessage(SocketMessage msg)
        {
            await msg.Channel.SendMessageAsync("I'm sorry, I didn't understand what you were asking me to do. Please try again.");
        }
    }
}