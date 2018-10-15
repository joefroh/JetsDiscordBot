using System;
using System.Collections.Generic;
using System.Linq;
using ClassLocator;
using Discord.WebSocket;

namespace discordBot
{
    class PollHandler
    {
        private List<IPoller> _pollers;

        private bool _pollersRunning = false;
        public PollHandler()
        {
            _pollers = new List<IPoller>();

            RegisterPollers();
        }

        private void RegisterPollers()
        {
            if (!Locator.Instance.Fetch<IConfigurationLoader>().Configuration.EnablePollers)
            {
                Locator.Instance.Fetch<ILogger>().LogLine("Pollers disabled in config.");
                return;
            }
            var pollers = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => typeof(IPoller).IsAssignableFrom(p) && !p.IsAbstract);
            foreach (var poller in pollers)
            {
                //TODO Handle conflicts like a good coder.
                var pollerInstance = Activator.CreateInstance(poller) as IPoller;
                _pollers.Add(pollerInstance);
                Locator.Instance.Fetch<ILogger>().LogLine("Poller Registered of type: " + pollerInstance.GetType());
            }
        }

        public void StartPollers()
        {
            if (_pollersRunning) return;

            foreach (var poller in _pollers)
            {
                poller.StartPoll();
            }

            _pollersRunning = true;
        }
    }
}