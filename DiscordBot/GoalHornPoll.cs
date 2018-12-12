using System;
using System.Collections.Generic;
using ClassLocator;
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
            if (null == Locator.Instance.Fetch<IConfigurationLoader>().Configuration.GoalHornConfig)
                return;
                
            foreach (var goalHornConfig in Locator.Instance.Fetch<IConfigurationLoader>().Configuration.GoalHornConfig)
            {
                var guild = Locator.Instance.Fetch<DiscordSocketClient>().GetGuild(goalHornConfig.ServerID);
                if (null != guild)
                {
                    var channel = guild.GetTextChannel(goalHornConfig.TargetChannelID);

                    if (null != channel)
                    {
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