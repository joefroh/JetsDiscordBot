using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLocator;

namespace DiscordBot
{
    public class EventBroker : IEventBroker
    {
        Dictionary<Type, List<IEventHandler>> _handlerDict;

        public EventBroker()
        {
            _handlerDict = new Dictionary<Type, List<IEventHandler>>();
            RegisterEventHandlers();
        }

        private void RegisterEventHandlers()
        {
            var handlers = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => typeof(IEventHandler).IsAssignableFrom(p) && !p.IsInterface);
            foreach (var handler in handlers)
            {
                var eventHandler = Activator.CreateInstance(handler) as IEventHandler;
                this.RegisterHandler(eventHandler);

                Locator.Instance.Fetch<ILogger>().LogLine("Registering Event Handler: " + eventHandler.GetType());
            }
        }

        public async Task FireEvent(IEvent firedEvent)
        {
            List<IEventHandler> handlerList;

            // Find the channel if it exists and fire all the handlers
            if (_handlerDict.TryGetValue(firedEvent.Channel, out handlerList))
            {
                foreach (var handler in handlerList)
                {
                    await handler.Fire(firedEvent);
                }
            }
        }

        public void RegisterHandler(IEventHandler handler)
        {
            List<IEventHandler> handlerlist;

            // Check if we already have a channel listed
            if (_handlerDict.ContainsKey(handler.Channel))
            {
                handlerlist = _handlerDict[handler.Channel];

                // check if we already have this handler registered
                if (handlerlist.Contains(handler))
                {
                    // We already have this handler registered to that channel
                    throw new ArgumentException(String.Format("Handler is already registered."), nameof(handler));
                }
            }
            else
            {
                // We don't yet have a handler list registered for that channel, make one.
                handlerlist = new List<IEventHandler>();
                _handlerDict.Add(handler.Channel, handlerlist);
            }

            handlerlist.Add(handler);
        }
    }
}