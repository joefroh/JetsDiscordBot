using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHLApi;

namespace discordBot
{
    public class NHLInformationSummarizer
    {
        public static string GameSummary(ScheduleData scheduleData, GameScheduleData nextGame, TeamDetail teamData)
        {
            // Final Score: <Home Team> X, <Away Team> Y
            // Next Game: <Next Game for Config'd Team>
            var builder = new StringBuilder();
            builder.AppendLine(String.Format("Final Score: {0} {1}, {2} {3}", scheduleData.Dates[0].Games[0].Teams.Home.Team.Name, scheduleData.Dates[0].Games[0].Teams.Home.Score, scheduleData.Dates[0].Games[0].Teams.Away.Team.Name, scheduleData.Dates[0].Games[0].Teams.Away.Score));
            builder.AppendLine(NextGameSummary(nextGame, teamData));

            return builder.ToString();
        }

        public static string PeriodSummary(LineScorePeriod lineScorePeriod, TeamDetail home, TeamDetail away, NHLApi.GameDetail gameDetail)
        {
            /*
                Period X:
                Shots: <HomeTeam> X, <AwayTeam> Y
                Goals: <HomeTeam> X, <AwayTeam> Y
                Scoring Plays:
                .
                .
                .
             */
            var playIndexStart = gameDetail.LiveData.Plays.PlaysByPeriod[lineScorePeriod.Num - 1].StartIndex;
            var playIndexEnd = gameDetail.LiveData.Plays.PlaysByPeriod[lineScorePeriod.Num - 1].EndIndex;

            var builder = new StringBuilder();
            builder.AppendLine(String.Format("**Period {0}:**", lineScorePeriod.Num));
            builder.AppendLine(String.Format("__Shots:__ {0} {1}, {2} {3}", home.Abbreviation, lineScorePeriod.Home.ShotsOnGoal, away.Abbreviation, lineScorePeriod.Away.ShotsOnGoal));
            builder.AppendLine(String.Format("__Goals:__ {0} {1}, {2} {3}", home.Abbreviation, lineScorePeriod.Home.Goals, away.Abbreviation, lineScorePeriod.Away.Goals));



            var scoringPlays = GetScoringPlaysForPeriod(gameDetail, playIndexStart, playIndexEnd);

            if (scoringPlays.Count() > 0)
            {
                builder.AppendLine("__Scoring Plays:__");

                foreach (var play in scoringPlays)
                {
                    builder.AppendLine(" - " + play);
                }
            }
            else
            {
                builder.AppendLine(" - No Goals were scored this period.");
            }

            return builder.ToString();
        }

        public static string NextGameSummary(GameScheduleData nextGame, TeamDetail teamData)
        {
            var game = nextGame.Dates[0].Games[0];
            var gamelocaltime = game.GameDate.AddHours(teamData.Venue.TimeZone.Offset);
            var gameTime = gamelocaltime.ToLongDateString() +" at " + gamelocaltime.ToShortTimeString() + " "+ teamData.Venue.TimeZone.TZ;
            return string.Format("The next {0} game is {1}, {2} @ {3} at {4}", teamData.Name,gameTime , game.Teams.Away.Team.Name, game.Teams.Home.Team.Name, game.Venue.Name);
        }

        #region private helpers
        private static IEnumerable<String> GetScoringPlaysForPeriod(NHLApi.GameDetail gameDetail, int start, int end)
        {
            List<String> result = new List<String>();

            foreach (var play in gameDetail.LiveData.Plays.ScoringPlays)
            {
                if (play >= start && play <= end)
                {
                    result.Add(gameDetail.LiveData.Plays.AllPlays[play].Result.Description);
                }
            }

            return result;
        }
        #endregion
    }
}