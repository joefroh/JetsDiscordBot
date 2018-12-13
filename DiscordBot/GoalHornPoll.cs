using System;
using System.Collections.Generic;
using ClassLocator;
using Discord;
using Discord.WebSocket;

namespace DiscordBot
{
    class GoalHornPoll : IPoller
    {
        private List<GoalHorn> _goalHorns;
        public GoalHornPoll()
        {
            _goalHorns = new List<GoalHorn>();
        }

        private void RegisterGoalHorns()
        {
            Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Starting to register Goal Horns"));
            if (null == Locator.Instance.Fetch<IConfigurationLoader>().Configuration.GoalHornConfig)
            {
                Locator.Instance.Fetch<ILogger>().LogLine("WARNING: No Goal Horns found in config");
                return;
            }

            foreach (var goalHornConfig in Locator.Instance.Fetch<IConfigurationLoader>().Configuration.GoalHornConfig)
            {
                var guild = Locator.Instance.Fetch<IDiscordClient>().GetGuildAsync(goalHornConfig.ServerID).Result;
                if (null != guild)
                {
                    var channel = guild.GetTextChannelAsync(goalHornConfig.TargetChannelID).Result;

                    if (null != channel)
                    {
                         Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Regisering Goal Horn for {0}", goalHornConfig.TeamFriendlyName));
                        _goalHorns.Add(new GoalHorn(channel as SocketTextChannel, goalHornConfig));
                    }
                }
            }
        }

        public void StartPoll()
        {
            RegisterGoalHorns();
            foreach (var goalHorn in _goalHorns)
            {
                goalHorn.Run();
            }
        }
    }
}