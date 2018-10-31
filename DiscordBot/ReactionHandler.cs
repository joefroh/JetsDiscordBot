using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLocator;
using Discord.WebSocket;

namespace discordBot
{
    public class ReactionHandler
    {
        Dictionary<string, IReactionActor> _reactions;
        public ReactionHandler()
        {
            _reactions = new Dictionary<string, IReactionActor>();
            LoadReactions();
        }
        private void LoadReactions()
        {
            if (!Locator.Instance.Fetch<IConfigurationLoader>().Configuration.EnableReactions)
            {
                Locator.Instance.Fetch<ILogger>().LogLine("Reactions disabled in config.");
                return;
            }

            var reactors = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => typeof(IReactionActor).IsAssignableFrom(p) && !p.IsAbstract);
            foreach (var reactor in reactors)
            {
                //TODO Handle conflicts like a good coder.
                var reactionActor = Activator.CreateInstance(reactor) as IReactionActor;
                _reactions.Add(reactionActor.TriggerString, reactionActor);
                Locator.Instance.Fetch<ILogger>().LogLine("Registering reaction actor for string: " + reactionActor.TriggerString);
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
    }
}