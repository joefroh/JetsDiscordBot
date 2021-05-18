using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLocator;
using Discord.WebSocket;

namespace DiscordBot
{
    public class ReactionHandler : IEventHandler
    {
        Dictionary<string, ReactionActor> _reactions;

        public Type Channel { get { return typeof(MessageReceivedEvent); } }

        public ReactionHandler()
        {
            _reactions = new Dictionary<string, ReactionActor>();
            LoadReactions();
        }
        private void LoadReactions()
        {
            if (!Locator.Instance.Fetch<IConfigurationLoader>().Configuration.EnableReactions)
            {
                Locator.Instance.Fetch<ILogger>().LogLine("Reactions disabled in config.");
                return;
            }

            foreach (var reactionConfig in Locator.Instance.Fetch<IConfigurationLoader>().Configuration.ReactionActorConfig)
            {
                var reaction = new ReactionActor(reactionConfig);
                _reactions.Add(reactionConfig.Trigger, reaction);
            }
        }

        public async Task HandleMessage(SocketMessage msg)
        {
            var keys = _reactions.Keys;

            foreach (var key in keys)
            {
                if (msg.Content.ToLower().Contains(key))
                {
                    await _reactions[key].React(msg);
                }
            }
        }

        public async Task Fire(IEvent firedEvent)
        {
            await HandleMessage((firedEvent as MessageReceivedEvent).Message);
        }
    }
}