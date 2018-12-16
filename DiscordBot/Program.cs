using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClassLocator;

namespace DiscordBot
{
    class Program
    {
        // main
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        // private members
        private DiscordBot _bot;
        private PollHandler _pollHandler;

        // async main
        public async Task MainAsync()
        {
            await StartupDiscordBot();
            Locator.Instance.Fetch<ILogger>().LogLine("Awaiting main thread.");
            await Task.Delay(-1);
        }

        public async Task StartupDiscordBot()
        {
            Locator.Instance.Fetch<ILogger>().LogLine("Starting up the bot.");
            _bot = new DiscordBot();
            await _bot.LoginAsync();
            await _bot.StartAsync();
        }

        public void StartupPollers()
        {
            _pollHandler = new PollHandler();
            _pollHandler.StartPollers();
        }
    }
}
