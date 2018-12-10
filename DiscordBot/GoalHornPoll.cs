using System;
using System.Collections.Generic;
using ClassLocator;
using Discord.WebSocket;

namespace discordBot
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
                var guild = Locator.Instance.Fetch<DiscordSocketClient>().GetGuild(goalHornConfig.ServerID);
                if (null != guild)
                {
                    var channel = guild.GetTextChannel(goalHornConfig.TargetChannelID);

                    if (null != channel)
                    {
                        Locator.Instance.Fetch<ILogger>().LogLine(String.Format("Regisering Goal Horn for {0}", goalHornConfig.TeamFriendlyName));
                        _goalHorns.Add(new GoalHorn(channel, goalHornConfig));
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