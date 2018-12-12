using System;
using System.Collections.Generic;

namespace discordBot {
    public class EventBroker : IEventBroker {
        Dictionary<string, List<IEventHandler>> _handlerDict;

        public EventBroker () {
            _handlerDict = new Dictionary<string, List<IEventHandler>> ();
        }
        public void FireEvent (IEvent firedEvent) {
            List<IEventHandler> handlerList;

            // Find the channel if it exists and fire all the handlers
            if (_handlerDict.TryGetValue (firedEvent.Channel, out handlerList)) {
                foreach (var handler in handlerList) {
                    handler.Fire (firedEvent); //TODO: Can probably async this
                }
            }
        }

        public void RegisterHandler (IEventHandler handler) {
            List<IEventHandler> handlerlist;

            // Check if we already have a channel listed
            if (_handlerDict.ContainsKey (handler.ChannelString)) {
                handlerlist = _handlerDict[handler.ChannelString];

                // check if we already have this handler registered
                if (handlerlist.Contains (handler)) {
                    // We already have this handler registered to that channel
                    throw new ArgumentException (String.Format ("Handler is already registered."), nameof (handler));
                }
            } else {
                // We don't yet have a handler list registered for that channel, make one.
                handlerlist = new List<IEventHandler> ();
                _handlerDict.Add (handler.ChannelString, handlerlist);
            }

            handlerlist.Add (handler);
        }
    }
}