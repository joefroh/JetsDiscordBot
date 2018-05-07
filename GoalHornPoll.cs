using System;
using System.Collections.Generic;
using Discord.WebSocket;

namespace discordBot
{
    class GoalHornPoll : IPoller
    {
        private List<GoalHorn> _goalHorns;
        public GoalHornPoll(DiscordSocketClient client, Configuration config) : base(client, config)
        {
            _goalHorns = new List<GoalHorn>();
        }

        private void RegisterGoalHorns()
        {
            foreach (var goalHornConfig in _config.GoalHornConfig)
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