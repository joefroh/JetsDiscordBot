using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHLApi;

namespace discordBot
{
    public class PeriodSummaryGenerator
    {
        LineScorePeriod _periodData;
        TeamDetail _home;
        TeamDetail _away;
        NHLApi.GameDetail _gameDetail;
        int _playIndexStart;
        int _playIndexEnd;

        public PeriodSummaryGenerator(LineScorePeriod lineScorePeriod, TeamDetail home, TeamDetail away, NHLApi.GameDetail gameDetail)
        {
            _periodData = lineScorePeriod;
            _home = home;
            _away = away;
            _gameDetail = gameDetail;

            _playIndexStart = gameDetail.LiveData.Plays.PlaysByPeriod[_periodData.Num - 1].StartIndex;
            _playIndexEnd = gameDetail.LiveData.Plays.PlaysByPeriod[_periodData.Num - 1].EndIndex;
        }

        public override string ToString()
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
            var builder = new StringBuilder();
            builder.AppendLine(String.Format("**Period {0}:**", _periodData.Num));
            builder.AppendLine(String.Format("__Shots:__ {0} {1}, {2} {3}", _home.Abbreviation, _periodData.Home.ShotsOnGoal, _away.Abbreviation, _periodData.Away.ShotsOnGoal));
            builder.AppendLine(String.Format("__Goals:__ {0} {1}, {2} {3}", _home.Abbreviation, _periodData.Home.Goals, _away.Abbreviation, _periodData.Away.Goals));

            var scoringPlays = GetScoringPlaysForPeriod(_gameDetail);

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

        private IEnumerable<String> GetScoringPlaysForPeriod(NHLApi.GameDetail gameDetail)
        {
            List<String> result = new List<String>();

            foreach (var play in gameDetail.LiveData.Plays.ScoringPlays)
            {
                if (play >= _playIndexStart && play <= _playIndexEnd)
                {
                    result.Add(gameDetail.LiveData.Plays.AllPlays[play].Result.Description);
                }
            }

            return result;
        }
    }
}