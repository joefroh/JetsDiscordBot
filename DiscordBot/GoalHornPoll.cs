using System;
using System.Collections.Generic;
using ClassLocator;
using Discord.WebSocket;

namespace discordBot
{
    class GoalHornPoll : IPoller
    {
        private List<GoalHorn> _goalHorns;
        public GoalHornPoll(DiscordSocketClient client) : base(client)
        {
            _goalHorns = new List<GoalHorn>();
        }

        private void RegisterGoalHorns()
        {
            if (null == Locator.Instance.Fetch<IConfigurationLoader>().Configuration.GoalHornConfig)
                return;
                
            foreach (var goalHornConfig in Locator.Instance.Fetch<IConfigurationLoader>().Configuration.GoalHornConfig)
            {
                var guild = _client.GetGuild(goalHornConfig.ServerID);
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

        public override void StartPoll()
        {
            RegisterGoalHorns();
            foreach (var goalHorn in _goalHorns)
            {
                goalHorn.Run();
            }
        }
    }
}